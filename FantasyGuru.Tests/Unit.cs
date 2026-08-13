using FantasyGuru.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FantasyGuru.Tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void TestMethod1()
        {
            var myTeam = new List<Player> { new Player { id = 1 }, new Player { id = 2 } };
            var oppTeam = new List<Player> { new Player { id = 2 }, new Player { id = 3 } };

            var myUnique = myTeam.Where(p => !oppTeam.Any(o => o.id == p.id)).ToList();

            Assert.AreEqual(1, myUnique.Count);
            Assert.AreEqual(1, myUnique[0].id);
        }
    }
}
