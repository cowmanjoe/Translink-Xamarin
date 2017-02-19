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
            Assert.IsTrue(Util.RouteEquals("", "")); 
        }

        [TestMethod]
        public void TestRouteEquals_JustNumbers_Equal()
        {
            Assert.IsTrue(Util.RouteEquals("004", "4"));
            Assert.IsTrue(Util.RouteEquals("04", "4"));
            Assert.IsTrue(Util.RouteEquals("4", "4"));
            Assert.IsTrue(Util.RouteEquals("00033415", "033415"));
        }


        [TestMethod]
        public void TestRouteEquals_NumbersAndLetters_Equal()
        {
            Assert.IsTrue(Util.RouteEquals("N17", "N17"));
            Assert.IsTrue(Util.RouteEquals("250A", "0250A"));
            Assert.IsTrue(Util.RouteEquals("N03A", "N3A"));
        }

        [TestMethod]
        public void TestRouteEquals_DifferentCase_Equal()
        {
            Assert.IsTrue(Util.RouteEquals("N17", "n17"));
            Assert.IsTrue(Util.RouteEquals("250Aa", "250aA"));
            Assert.IsTrue(Util.RouteEquals("n3a", "N003A")); 
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestRouteEquals_EmptyAndZero_FormatException()
        {
            Assert.IsFalse(Util.RouteEquals("", "0"));
            Assert.IsFalse(Util.RouteEquals("", "00000")); 
        }

        [TestMethod]
        public void TestRouteEquals_NumbersAndLetters_NotEqual()
        {
            Assert.IsFalse(Util.RouteEquals("N17", "A17"));
            Assert.IsFalse(Util.RouteEquals("1234", "BBD5JKJF"));
            Assert.IsFalse(Util.RouteEquals("N44D", "N44C")); 
        }

        
    }
}
