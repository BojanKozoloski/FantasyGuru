# FantasyGuru
 
FantasyGuru is an ASP.NET MVC (.NET Framework) web app for Fantasy Premier League (FPL) managers. It pulls data from the official FPL API and lets you view your squad, browse league standings, and compare your team against an opponent's — all rendered on an interactive pitch layout.
 
> **Note:** Some data (squad picks, gameweek points, league standings) is currently backed by hardcoded test data rather than live FPL endpoints, since the FPL season data isn't available year-round. See [Status](#status) below.
 
## Features
- **Guru view** — the starting page where you enter your FPL manager ID.
- **Squad view** — see your starting XI laid out on a pitch by position (GK/DEF/MID/FWD), plus your 4 bench players in a separate strip. Click a player card to toggle between gameweek points and total points.There is also a choose league selection option where you choose which league you want to view 
- **LeagueC view** — view selected league leaderboard in a scrollable table, with a search bar to filter by manager or team name, and Next/Previous buttons for large leagues.
- **Compare view** — see the players that differ between your squad and an opponent's, laid out on opposite halves of the pitch (starters and bench separately). A clickable scoreboard shows the points difference between you and your opponent, toggling between total points and gameweek points.
## Tech stack
 
- ASP.NET MVC (.NET Framework)
- Newtonsoft.Json for API deserialization
- Razor views (`.cshtml`)
- Vanilla JS/CSS for interactivity (no frontend framework)
- [Fantasy Premier League API](https://fantasy.premierleague.com/api/) as the data source

## Key FPL API endpoints used
 
| Endpoint | Purpose |
|---|---|
| `GET /api/entry/{id}/` | Manager profile, season total points, leagues |
| `GET /api/entry/{id}/event/{gw}/picks/` | Manager's squad picks + gameweek points for a given gameweek |
| `GET /api/bootstrap-static/` | Full player list with stats (points, team, position) |
| `GET /api/leagues-classic/{id}/standings/?page_standings={n}` | League leaderboard |
 
## Getting started
 
1. Clone the repo and open `FantasyGuru.sln` in Visual Studio.
2. Restore NuGet packages (Newtonsoft.Json is required).
3. Build and run (IIS Express).
4. Navigate to `/Manager/Guru` with any real FPL manager ID to view a squad (When you make an FPL account you can check the URL when you go to my team the number there represents your account).
## Status
 
All core data now comes from the live FPL API — manager profiles, squad picks, gameweek points, league standings, and chip status.
 
## Roadmap
 
- [ ] Show the top 5 players in a selected league by ownership percentage.
- [ ] Show fixture difficulty rating for players in the Compare view.
- [ ] Add suggested transfer/captaincy tips in the Compare view based on a formula.
- [ ] Track transfers made between gameweeks, and flag when a manager took a points hit for exceeding their free transfers.
- [ ] Add additional visualizations and charts (points history, rank trend over the season).
- [ ] Extend the responsive/mobile layout to the LeagueC and Guru views (Squad and Compare already have breakpoints).
- [ ] Investigate a native mobile app (.NET MAUI) sharing the existing `Models`/`FPLTeam` service layer.
