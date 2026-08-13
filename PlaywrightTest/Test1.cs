namespace PlaywrightTest
{
    [TestClass]
    
    public class SquadPageTests : PageTest
    {
        [TestMethod]
        public async Task SquadPage_DisplaysPlayerCards()
        {
            await Page.GotoAsync("https://localhost:44341/Manager/Squad?id=1");

            var playerCards = Page.Locator(".player-card");
            await Expect(playerCards.First).ToBeVisibleAsync();

            var count = await playerCards.CountAsync();
            Assert.IsTrue(count > 0, "Expected at least one player card to be visible on the squad page.");
        }
    }
}