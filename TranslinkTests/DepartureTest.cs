using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Translink;

namespace TranslinkTests
{
    [TestClass]
    public class DepartureTest
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestRouteEquals_EmptyStrings_Equal()
        {
            Assert.IsTrue(Departure.RouteEquals("", "")); 
        }

        [TestMethod]
        public void TestRouteEquals_JustNumbers_Equal()
        {
            Assert.IsTrue(Departure.RouteEquals("004", "4"));
            Assert.IsTrue(Departure.RouteEquals("04", "4"));
            Assert.IsTrue(Departure.RouteEquals("4", "4"));
            Assert.IsTrue(Departure.RouteEquals("00033415", "033415"));
        }


        [TestMethod]
        public void TestRouteEquals_NumbersAndLetters_Equal()
        {
            Assert.IsTrue(Departure.RouteEquals("N17", "N17"));
            Assert.IsTrue(Departure.RouteEquals("250A", "0250A"));
            Assert.IsTrue(Departure.RouteEquals("N03A", "N3A"));
        }

        [TestMethod]
        public void TestRouteEquals_DifferentCase_Equal()
        {
            Assert.IsTrue(Departure.RouteEquals("N17", "n17"));
            Assert.IsTrue(Departure.RouteEquals("250Aa", "250aA"));
            Assert.IsTrue(Departure.RouteEquals("n3a", "N003A")); 
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestRouteEquals_EmptyAndZero_FormatException()
        {
            Assert.IsFalse(Departure.RouteEquals("", "0"));
            Assert.IsFalse(Departure.RouteEquals("", "00000")); 
        }

        [TestMethod]
        public void TestRouteEquals_NumbersAndLetters_NotEqual()
        {
            Assert.IsFalse(Departure.RouteEquals("N17", "A17"));
            Assert.IsFalse(Departure.RouteEquals("1234", "BBD5JKJF"));
            Assert.IsFalse(Departure.RouteEquals("N44D", "N44C")); 
        }

        
    }
}
