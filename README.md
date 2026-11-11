# FantasyGuru

FantasyGuru is an ASP.NET Core MVC web app for Fantasy Premier League (FPL) managers. It pulls live data from the official FPL API and lets you view your squad, browse league standings, and compare your team against an opponent's, all rendered on an interactive pitch layout.

## Features

- **Guru view**: the starting page where you enter your FPL manager ID.
- **Squad view**: your starting XI laid out on a pitch by position (GK/DEF/MID/FWD), with your bench in a separate strip. The current gameweek is detected automatically. Click a player card to toggle between gameweek points and total points, click your info box to toggle gameweek and total points for the whole squad, and use the league dropdown to jump to any classic league you're in. Captains are badged, and captain multipliers are applied to gameweek points.
- **LeagueC view**: a league leaderboard showing 50 managers per page, with Next/Previous navigation.
  - The GW and Total columns show each manager's points **relative to you** (+ or -), rather than raw points. Colour shows who is ahead.
  - Sorted by total points by default, with click-to-resort on either column.
  - A search bar filters by manager or team name on the current page.
  - Your own points (gameweek and total) are shown above the table.
  - If you're not in the visible 50, a pinned row at the bottom shows your team, your rank, and your points. If you are in the top 50, your row is highlighted in place and no extra row appears.
  - Your own row shows "You" instead of a Compare link.
- **Compare view**: shows the players that differ between your squad and an opponent's, on opposite halves of the pitch (starters and bench separately).
  - Captaincy is part of the diff, so the same player captained by only one of you still shows up as a differential.
  - A clickable scoreboard toggles between the gameweek and total points difference (gameweek is shown first).
  - Active and available chips (Wildcard, Free Hit, Bench Boost, Triple Captain) are shown for both managers. When Bench Boost is active, all 15 players are shown on the pitch.
  - A **differential picker** at the bottom centre suggests the top 3 players per position (GK / DEF / MID / FWD) that neither manager owns, ranked by FPL's `form` stat. Switch positions with the tabs.

## Tech stack

- ASP.NET Core MVC (migrated from ASP.NET MVC on .NET Framework)
- `System.Text.Json` for API deserialization, with case-insensitive property matching
- Razor views (`.cshtml`)
- Vanilla JS/CSS for interactivity (no frontend framework)
- [Fantasy Premier League API](https://fantasy.premierleague.com/api/) as the data source

### Why .NET Core

The project was migrated from .NET Framework so it can be containerised with Docker using standard Linux images. Framework projects need Windows containers, which are much heavier and harder to work with.

One migration gotcha worth remembering: `System.Text.Json` is case-sensitive by default and Newtonsoft was not. Every deserialization call in `FPLTeam.cs` passes a shared `JsonSerializerOptions { PropertyNameCaseInsensitive = true }` so properties like `League.Id` and `League.Name` match the API's lowercase `id` and `name`. Property types must also match the JSON exactly. For example, `entry_rank` is a number, so it is declared `int?`, not `string`.

## Project structure

```
FantasyGuruC/
├── Controllers/
│   └── ManagerController.cs      # Guru, Squad, LeagueC, Compare actions
├── Services/
│   └── FPLTeam.cs                 # All FPL API calls and the logic built on them
├── Models/
│   ├── Player.cs
│   ├── Manager.cs
│   ├── League.cs / Leagues.cs
│   ├── LeagueStandings.cs / StandingsTable.cs / StandingsResult.cs
│   ├── Compare.cs
│   ├── Pick.cs / PickR.cs / EntryHistory.cs
│   ├── Chip.cs / ManagerHistory.cs
│   ├── Event.cs
│   └── Bootstrap.cs
└── Views/
    └── Manager/
        ├── Guru.cshtml
        ├── Squad.cshtml
        ├── LeagueC.cshtml
        └── Compare.cshtml
```

## What `FPLTeam` does

| Method | Purpose |
|---|---|
| `GetManager` | Manager profile, season points, ranks, and leagues |
| `GetCurrentGameweek` | Finds the live gameweek from `bootstrap-static` (falls back to the next gameweek pre-season) |
| `GetSquad` | Builds a manager's 15 players for a gameweek, including position, captain flag, and multiplier |
| `CalculateGameweekPoints` | Sums `event_points * multiplier` for the starting 11 (all 15 under Bench Boost) |
| `GetChipStatus` | Works out which chips are available and which is active this gameweek |
| `GetLeagueStandings` | One raw page of a league leaderboard |
| `GetLeagueStandingsPaged` | Builds a 50-row page, stitching several API pages together if FPL returns fewer than 50 per request |
| `GetAllPlayers` / `GetTopDifferentials` | Fetches the full player pool once, then filters to players neither manager owns and ranks them by form |

## Key FPL API endpoints used

| Endpoint | Purpose |
|---|---|
| `GET /api/entry/{id}/` | Manager profile, season points, ranks, leagues |
| `GET /api/entry/{id}/event/{gw}/picks/` | A manager's squad picks for a gameweek (captain, multiplier) |
| `GET /api/entry/{id}/history/` | Chip usage history (used to compute available chips) |
| `GET /api/bootstrap-static/` | Full player list (points, team, position, form) and gameweek metadata |
| `GET /api/leagues-classic/{id}/standings/?page_standings={n}` | Paginated league leaderboard |

## Getting started

1. Clone the repo and open the solution in Visual Studio.
2. Make sure the .NET SDK matching the project's target framework is installed.
3. Restore packages and run (`F5`, or `dotnet run` from the project folder).
4. Go to `/Manager/Guru` and enter a real FPL manager ID. Once you've made an FPL account, your manager ID is the number in the URL when you open "My Team".

## Testing

- **Unit tests** (MSTest): pure logic with no network calls, covering unique-player diffing, captain-aware differentials, starter/bench splitting, bench ordering, gameweek point calculation (including captain and Bench Boost), and league sort-before-paginate behaviour.
- **Integration tests** (MSTest): hit the real FPL API to confirm `FPLTeam` returns well-formed data (manager profile, squad, league standings).
- **End-to-end tests** (Playwright + MSTest): drive a real browser through Guru, Squad, LeagueC, and Compare, covering search, pagination, and the point toggles. The app must be running first.

Note: these suites were written before the .NET Core migration, so their project references, target frameworks, and the localhost port used by Playwright need to point at the new project.

## Versioning

Releases are tagged on GitHub using [Semantic Versioning](https://semver.org/) (`MAJOR.MINOR.PATCH`). The .NET Core migration is a good candidate for a major version bump. See the repo's **Releases** page for what shipped in each version.

## Known limitations

- **Chip reset boundary:** the second set of chips is assumed to start after Gameweek 19 (the 2025/26 rule). It is a single constant in `FPLTeam.cs`, so re-check it each season.
- **Gameweek points are computed, not read from FPL:** this avoids a second API call per manager, but it doesn't subtract transfer point hits.
- **League sorting is per page:** sorting and searching only apply to the 50 managers currently loaded, because FPL only exposes leagues a page at a time. On huge leagues like Overall, fetching everything isn't practical. Your own rank comes from your profile data instead.
- **League page speed:** if FPL returns fewer than 50 entries per request, building one 50-row page takes several API calls.
- **`entry_rank` can be null** for very large leagues, in which case the pinned row omits your rank.

## Roadmap

- [ ] Add a Dockerfile and run the app in a container.
- [ ] Show the top 5 players in a selected league by ownership percentage.
- [ ] Show fixture difficulty rating for players in the Compare view.
- [ ] Add suggested transfer and captaincy tips in the Compare view based on a formula.
- [ ] Track transfers made between gameweeks, and flag points hits.
- [ ] Add additional visualizations and charts (points history, rank trend).
- [ ] Extend the responsive/mobile layout across all views.
- [ ] Investigate a native mobile app (.NET MAUI) sharing the existing models and `FPLTeam` service.
