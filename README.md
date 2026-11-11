# FantasyGuru

FantasyGuru is an ASP.NET Core MVC web app for Fantasy Premier League (FPL) managers. It pulls live data from the official FPL API and lets you view your squad, browse league standings, and compare your team against an opponent's, all rendered on an interactive pitch layout.

## Features

- **Guru view**: the starting page where you enter your FPL manager ID.
- **Squad view**: your starting XI laid out on a pitch by position (GK/DEF/MID/FWD), with your bench in a separate strip. The current gameweek is detected automatically. Click a player card to toggle between gameweek points and total points, click your info box to toggle gameweek and total points for the whole squad, and use the league dropdown to jump to any league you're in. Captains are badged, and captain multipliers are applied to gameweek points.
- **LeagueC view**: a league leaderboard with Next/Previous navigation.
  - The GW and Total columns show each manager's points **relative to you** (+ or -), rather than raw points. Colour shows who is ahead.
  - Sorted by total points by default, with click-to-resort on either column.
  - A search bar filters by manager or team name on the current page.
  - Your own points (gameweek and total) are shown above the table.
  - If you're not in the visible 50, a pinned row at the bottom shows your team, your rank, and your points. If you are in the top 50, your row is highlighted in place and no extra row appears.
  - Your own row shows "You" instead of a Compare link.
- **Compare view**: shows the players that differ between your squad and an opponent's, on opposite halves of the pitch (starters and bench separately).
  - Captaincy is part of the diff, so the same player captained by only one of you still shows up as a differential.
  - A clickable scoreboard toggles between the gameweek and total points difference (gameweek is shown first).
  - Active and available chips (Wildcard, Free Hit, Bench Boost, Triple Captain) are shown for both managers. 
  - A **differential picker** at the bottom centre suggests the top 3 players per position (GK / DEF / MID / FWD) that neither manager owns, ranked by FPL's `form` stat.

## Tech stack

- ASP.NET Core MVC (migrated from ASP.NET MVC on .NET Framework)
- Docker for application containerization
- Dockerfile for building and running the application in a container
- .dockerignore to exclude unnecessary files from the Docker build context
- `System.Text.Json` for API deserialization, with case-insensitive property matching
- Razor views (`.cshtml`)
- Vanilla JS/CSS for interactivity (no frontend framework)
- [Fantasy Premier League API](https://fantasy.premierleague.com/api/) as the data source

## Key FPL API endpoints used

| Endpoint | Purpose |
|---|---|
| `GET /api/entry/{id}/` | Manager profile, season points, ranks, leagues |
| `GET /api/entry/{id}/event/{gw}/picks/` | A manager's squad picks for a gameweek (captain, multiplier) |
| `GET /api/entry/{id}/history/` | Chip usage history (used to compute available chips) |
| `GET /api/bootstrap-static/` | Full player list (points, team, position, form) and gameweek metadata |
| `GET /api/leagues-classic/{id}/standings/?page_standings={n}` | Paginated league leaderboard |

## Getting started
 
### Prerequisites
 
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running (for the Docker route).
- Or the .NET SDK matching the project's target framework (for the local route).
### Running with Docker
 
1. Clone the repository:
```bash
   git clone https://github.com/BojanKozoloski/FantasyGuru.git
   cd FantasyGuruC
```
 
2. Build the Docker image:
```bash
   docker build -t fantasyguru .
```
 
3. Run the container:
```bash
   docker run -d -p 8080:8080 --name fantasyguru fantasyguru
```
 
4. Open your browser and go to `http://localhost:8080`.
5. Go to `/Manager/Guru` and enter a real FPL manager ID.
### Running locally with Visual Studio
 
Alternatively, you can run the application without Docker:
 
1. Install the .NET SDK matching the project's target framework.
2. Clone the repository and open the solution in Visual Studio.
3. Restore NuGet packages.
4. Run the application using Visual Studio (`F5`) or `dotnet run` from the project folder.
5. Navigate to `/Manager/Guru`.
### Finding your manager ID
 
Once you've made an FPL account, your manager ID is the number in the URL when you open "My Team".

## Testing

- **Unit tests** (MSTest): pure logic with no network calls, covering unique-player diffing, captain-aware differentials, starter/bench splitting, bench ordering, gameweek point calculation (including captain and Bench Boost), and league sort-before-paginate behaviour.
- **Integration tests** (MSTest): hit the real FPL API to confirm `FPLTeam` returns well-formed data (manager profile, squad, league standings).
- **End-to-end tests** (Playwright + MSTest): drive a real browser through Guru, Squad, LeagueC, and Compare, covering search, pagination, and the point toggles. The app must be running first.

Note: these suites were written before the .NET Core migration, so their project references, target frameworks, and the localhost port used by Playwright need to point at the new project.

## Versioning

Releases are tagged on GitHub using [Semantic Versioning](https://semver.org/) (`MAJOR.MINOR.PATCH`). The .NET Core migration is a good candidate for a major version bump. See the repo's **Releases** page for what shipped in each version.

## Roadmap

- [ ] Show the top 5 players in a selected league by ownership percentage.
- [ ] Show fixture difficulty rating for players in the Compare view.
- [ ] Track transfers made between gameweeks, and flag points hits.
- [ ] Add additional visualizations and charts (points history, rank trend).
- [ ] Extend the responsive/mobile layout across all views.
- [ ] Investigate a native mobile app (.NET MAUI) sharing the existing models and `FPLTeam` service.
