using FantasyGuru.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FantasyGuru.IntegrationTests
{
    [TestClass]
    public class FPLTeamIntegrationTests
    {
        // Use a real, known FPL manager id for these tests
        private const int KnownManagerId = 170674;

        [TestMethod]
        public void GetManager_ReturnsValidManagerFromRealApi()
        {
            var service = new FPLTeam();
            var manager = service.GetManager(KnownManagerId);

            Assert.IsNotNull(manager);
            Assert.IsFalse(string.IsNullOrEmpty(manager.player_first_name));
            Assert.IsFalse(string.IsNullOrEmpty(manager.player_last_name));
        }

        [TestMethod]
        public void GetManager_ReturnsLeaguesData()
        {
            var service = new FPLTeam();
            var manager = service.GetManager(KnownManagerId);

            Assert.IsNotNull(manager.leagues);
            Assert.IsNotNull(manager.leagues.classic);
        }

        [TestMethod]
        public void GetSquad_ReturnsFifteenPlayers()
        {
            var service = new FPLTeam();
            var squad = service.GetSquad(1); // uses your hardcoded id==1 branch

            Assert.AreEqual(15, squad.Count);
        }

        [TestMethod]
        public void GetSquad_AllPlayersHaveNamesFromBootstrapStatic()
        {
            var service = new FPLTeam();
            var squad = service.GetSquad(1);

            Assert.IsTrue(squad.All(p => !string.IsNullOrEmpty(p.web_name)));
        }

        [TestMethod]
        public void GetSquad_PositionsAreAssignedOneToFifteen()
        {
            var service = new FPLTeam();
            var squad = service.GetSquad(1);

            var positions = squad.Select(p => p.position).OrderBy(p => p).ToList();

            for (int i = 0; i < 15; i++)
            {
                Assert.AreEqual(i + 1, positions[i]);
            }
        }

        [TestMethod]
        public void GetLeagueStandings_ReturnsResultsForKnownLeague()
        {
            var service = new FPLTeam();
            var standings = service.GetLeagueStandings(314); // FPL's "Overall" league id

            Assert.IsNotNull(standings);
            Assert.IsNotNull(standings.league);
            Assert.IsNotNull(standings.standings);
        }
    }
}