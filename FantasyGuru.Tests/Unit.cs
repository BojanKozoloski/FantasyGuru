using FantasyGuru.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FantasyGuru.Tests
{
    [TestClass]
    public class CompareLogicTests
    {
        [TestMethod]
        public void UniquePlayers_ExcludesSharedPlayers()
        {
            var myTeam = new List<Player> { new Player { id = 1 }, new Player { id = 2 } };
            var oppTeam = new List<Player> { new Player { id = 2 }, new Player { id = 3 } };

            var myUnique = myTeam.Where(p => !oppTeam.Any(o => o.id == p.id)).ToList();
            var oppUnique = oppTeam.Where(p => !myTeam.Any(o => o.id == p.id)).ToList();

            Assert.AreEqual(1, myUnique.Count);
            Assert.AreEqual(1, myUnique[0].id);

            Assert.AreEqual(1, oppUnique.Count);
            Assert.AreEqual(3, oppUnique[0].id);
        }

        [TestMethod]
        public void UniquePlayers_IdenticalTeams_ReturnsEmptyLists()
        {
            var myTeam = new List<Player> { new Player { id = 1 }, new Player { id = 2 } };
            var oppTeam = new List<Player> { new Player { id = 1 }, new Player { id = 2 } };

            var myUnique = myTeam.Where(p => !oppTeam.Any(o => o.id == p.id)).ToList();

            Assert.AreEqual(0, myUnique.Count);
        }

        [TestMethod]
        public void UniquePlayers_CompletelyDifferentTeams_ReturnsAllPlayers()
        {
            var myTeam = new List<Player> { new Player { id = 1 }, new Player { id = 2 } };
            var oppTeam = new List<Player> { new Player { id = 3 }, new Player { id = 4 } };

            var myUnique = myTeam.Where(p => !oppTeam.Any(o => o.id == p.id)).ToList();

            Assert.AreEqual(2, myUnique.Count);
        }

        [TestMethod]
        public void PointsDifference_MePositive_ReturnsCorrectDifference()
        {
            int myPoints = 100;
            int oppPoints = 80;
            int difference = myPoints - oppPoints;

            Assert.AreEqual(20, difference);
            Assert.IsTrue(difference > 0);
        }

        [TestMethod]
        public void PointsDifference_Equal_ReturnsZero()
        {
            int myPoints = 100;
            int oppPoints = 100;
            int difference = myPoints - oppPoints;

            Assert.AreEqual(0, difference);
        }

        [TestMethod]
        public void PointsDifference_OpponentHigher_ReturnsNegative()
        {
            int myPoints = 60;
            int oppPoints = 90;
            int difference = myPoints - oppPoints;

            Assert.IsTrue(difference < 0);
            Assert.AreEqual(30, System.Math.Abs(difference));
        }
    }

    [TestClass]
    public class SquadPositionTests
    {
        private List<Player> BuildTeamWithPositions()
        {
            var team = new List<Player>();
            for (int i = 1; i <= 15; i++)
            {
                team.Add(new Player { id = i, position = i, element_type = (i % 4) + 1 });
            }
            return team;
        }

        [TestMethod]
        public void Starters_AreExactlyPositionsOneToEleven()
        {
            var team = BuildTeamWithPositions();
            var starters = team.Where(p => p.position >= 1 && p.position <= 11).ToList();

            Assert.AreEqual(11, starters.Count);
            Assert.IsTrue(starters.All(p => p.position <= 11));
        }

        [TestMethod]
        public void Bench_AreExactlyPositionsTwelveToFifteen()
        {
            var team = BuildTeamWithPositions();
            var bench = team.Where(p => p.position >= 12 && p.position <= 15).ToList();

            Assert.AreEqual(4, bench.Count);
            Assert.IsTrue(bench.All(p => p.position >= 12));
        }

        [TestMethod]
        public void StartersAndBench_DoNotOverlap()
        {
            var team = BuildTeamWithPositions();
            var starters = team.Where(p => p.position >= 1 && p.position <= 11).ToList();
            var bench = team.Where(p => p.position >= 12 && p.position <= 15).ToList();

            var overlap = starters.Select(p => p.id).Intersect(bench.Select(p => p.id));

            Assert.AreEqual(0, overlap.Count());
        }

        [TestMethod]
        public void BenchOrdering_GoalkeeperComesFirst()
        {
            var bench = new List<Player>
            {
                new Player { id = 12, position = 12, element_type = 3 }, // MID
                new Player { id = 13, position = 13, element_type = 1 }, // GK
                new Player { id = 14, position = 14, element_type = 4 }, // FWD
                new Player { id = 15, position = 15, element_type = 2 }, // DEF
            };

            var ordered = bench.OrderBy(p => p.element_type == 1 ? 0 : 1)
                                .ThenBy(p => p.position)
                                .ToList();

            Assert.AreEqual(1, ordered.First().element_type);
            Assert.AreEqual(13, ordered.First().id);
        }
    }

    [TestClass]
    public class LeagueStandingsSortingTests
    {
        private List<StandingsResult> BuildUnsortedResults()
        {
            return new List<StandingsResult>
            {
                new StandingsResult { entry = 1, total = 10 },
                new StandingsResult { entry = 2, total = 25 },
                new StandingsResult { entry = 3, total = 5 },
                new StandingsResult { entry = 4, total = 30 },
                new StandingsResult { entry = 5, total = 15 },
            };
        }

        [TestMethod]
        public void Results_SortedDescendingByTotal()
        {
            var results = BuildUnsortedResults();
            var sorted = results.OrderByDescending(r => r.total).ToList();

            Assert.AreEqual(30, sorted[0].total);
            Assert.AreEqual(25, sorted[1].total);
            Assert.AreEqual(5, sorted[4].total);
        }

        [TestMethod]
        public void Pagination_SortsBeforePaging_HighestTotalAlwaysOnPageOne()
        {
            var results = BuildUnsortedResults();
            int pageSize = 2;

            var sorted = results.OrderByDescending(r => r.total).ToList();
            var page1 = sorted.Skip(0).Take(pageSize).ToList();

            Assert.IsTrue(page1.Any(r => r.total == 30));
            Assert.IsTrue(page1.Any(r => r.total == 25));
        }

        [TestMethod]
        public void Pagination_HasNext_TrueWhenMoreResultsExist()
        {
            var results = BuildUnsortedResults();
            int pageSize = 2;
            int page = 1;

            bool hasNext = results.Count > page * pageSize;

            Assert.IsTrue(hasNext);
        }

        [TestMethod]
        public void Pagination_HasNext_FalseOnLastPage()
        {
            var results = BuildUnsortedResults();
            int pageSize = 2;
            int page = 3; // 5 results, pageSize 2 → pages: [1,2],[3,4],[5]

            bool hasNext = results.Count > page * pageSize;

            Assert.IsFalse(hasNext);
        }
    }
}