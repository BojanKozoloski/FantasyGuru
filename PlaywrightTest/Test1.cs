using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTest
{
    [TestClass]
    public class GuruPageTests : PageTest
    {
        private const string BaseUrl = "https://localhost:44341";

        [TestMethod]
        public async Task GuruPage_LoadsWithFormVisible()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Guru");

            await Expect(Page.Locator("input[name='id']")).ToBeVisibleAsync();
            await Expect(Page.Locator("button[type='submit']")).ToBeVisibleAsync();
        }

        [TestMethod]
        public async Task GuruPage_SubmittingIdNavigatesToSquad()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Guru");

            await Page.FillAsync("input[name='id']", "1");
            await Page.ClickAsync("button[type='submit']");

            await Expect(Page).ToHaveURLAsync(new Regex(".*Manager/Squad.*id=1"));
        }
    }

    [TestClass]
    public class SquadPageTests : PageTest
    {
        private const string BaseUrl = "https://localhost:44341";

        [TestMethod]
        public async Task SquadPage_DisplaysPlayerCards()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            var playerCards = Page.Locator(".player-card");
            await Expect(playerCards.First).ToBeVisibleAsync();

            var count = await playerCards.CountAsync();
            Assert.IsTrue(count > 0, "Expected at least one player card to be visible.");
        }

        [TestMethod]
        public async Task SquadPage_DisplaysExactlyElevenStarters()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            var starterCount = await Page.Locator(".team .player-card").CountAsync();
            Assert.AreEqual(11, starterCount);
        }

        [TestMethod]
        public async Task SquadPage_DisplaysExactlyFourBenchPlayers()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            var benchCount = await Page.Locator(".bench .bench-card").CountAsync();
            Assert.AreEqual(4, benchCount);
        }

        [TestMethod]
        public async Task SquadPage_ClickingPlayerCardTogglesPoints()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            var firstCard = Page.Locator(".player-card").First;
            var pointsSpan = firstCard.Locator(".player-points");

            var textBefore = await pointsSpan.InnerTextAsync();
            await firstCard.ClickAsync();
            var textAfter = await pointsSpan.InnerTextAsync();

            Assert.AreNotEqual(textBefore, textAfter);
        }

        [TestMethod]
        public async Task SquadPage_ClickingSquadInfoTogglesGwAndTotal()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            var squadInfo = Page.Locator(".squad-info");
            var pointsSpan = Page.Locator(".squad-points");

            var before = await pointsSpan.InnerTextAsync();
            await squadInfo.ClickAsync();
            var after = await pointsSpan.InnerTextAsync();

            Assert.AreNotEqual(before, after);
        }

        [TestMethod]
        public async Task SquadPage_SelectingLeagueNavigatesToLeagueC()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Squad?id=1");

            await Page.SelectOptionAsync("#league", new SelectOptionValue { Index = 1 });

            await Expect(Page).ToHaveURLAsync(new Regex(".*Manager/LeagueC.*"));
        }
    }

    [TestClass]
    public class LeaguePageTests : PageTest
    {
        private const string BaseUrl = "https://localhost:44341";

        [TestMethod]
        public async Task LeaguePage_DisplaysStandingsTable()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/LeagueC?managerId=1&leagueIndex=0");

            var rows = Page.Locator("#standingsTable tbody tr");
            var count = await rows.CountAsync();
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public async Task LeaguePage_SearchFiltersRows()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/LeagueC?managerId=1&leagueIndex=0");

            var totalBefore = await Page.Locator("#standingsTable tbody tr").CountAsync();

            await Page.FillAsync("#playerSearch", "Alice");

            var visibleRows = Page.Locator("#standingsTable tbody tr:visible");
            var visibleCount = await visibleRows.CountAsync();

            Assert.IsTrue(visibleCount > 0);
            Assert.IsTrue(visibleCount <= totalBefore);
        }

        [TestMethod]
        public async Task LeaguePage_NextButtonChangesResults()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/LeagueC?managerId=1&leagueIndex=0");

            var firstRowTextBefore = await Page.Locator("#standingsTable tbody tr").First
                .Locator(".player-name").InnerTextAsync();

            await Page.ClickAsync("text=Next →");

            var firstRowTextAfter = await Page.Locator("#standingsTable tbody tr").First
                .Locator(".player-name").InnerTextAsync();

            Assert.AreNotEqual(firstRowTextBefore, firstRowTextAfter);
        }

        [TestMethod]
        public async Task LeaguePage_CompareLinkNavigatesToComparePage()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/LeagueC?managerId=1&leagueIndex=0");

            await Page.ClickAsync(".compare a >> nth=0");

            await Expect(Page).ToHaveURLAsync(new Regex(".*Manager/Compare.*"));
        }
    }

    [TestClass]
    public class ComparePageTests : PageTest
    {
        private const string BaseUrl = "https://localhost:44341";

        [TestMethod]
        public async Task ComparePage_DisplaysUniquePlayersOnBothSides()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Compare?myid=1&oppid=2");

            var myCards = Page.Locator(".my-team .player-card");
            var oppCards = Page.Locator(".opponent-team .player-card");

            Assert.IsTrue(await myCards.CountAsync() > 0);
            Assert.IsTrue(await oppCards.CountAsync() > 0);
        }

        [TestMethod]
        public async Task ComparePage_ScoreboardTogglesOnClick()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Compare?myid=1&oppid=2");

            var scoreboard = Page.Locator(".scoreboard");
            var titleBefore = await Page.Locator("#scoreboardTitle").InnerTextAsync();

            await scoreboard.ClickAsync();

            var titleAfter = await Page.Locator("#scoreboardTitle").InnerTextAsync();
            Assert.AreNotEqual(titleBefore, titleAfter);
        }

        [TestMethod]
        public async Task ComparePage_ClickingPlayerCardTogglesPoints()
        {
            await Page.GotoAsync(BaseUrl + "/Manager/Compare?myid=1&oppid=2");

            var firstCard = Page.Locator(".player-card").First;
            var pointsSpan = firstCard.Locator(".player-points");

            var before = await pointsSpan.InnerTextAsync();
            await firstCard.ClickAsync();
            var after = await pointsSpan.InnerTextAsync();

            Assert.AreNotEqual(before, after);
        }
    }
}