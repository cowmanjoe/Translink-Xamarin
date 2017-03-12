using System;
using NUnit.Framework;
using Translink.Models;

namespace TranslinkTests
{
    [TestFixture]
    public class DateTimeTest
    {
        [TestCase(1000, 40, 20, 2, 10)]
        [TestCase(1000, 2, 41, 5, 39)]
        [TestCase(1422, 5, 12, 25, 0)]
        [TestCase(1, 4, 10, 12, 73)]
        [TestCase(1000, -4, 2, 10, 30)]
        [TestCase(1000, 5, -2, 10, 30)]
        [TestCase(1000, 5, 1, -5, 30)]
        [TestCase(1000, 5, 1, 5, -30)]
        public void TestConstructor_InvalidArguments(int year, int month, int day, int hour, int minute)
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => new Translink.Models.DateTime(year, month, day, hour, minute));
        }
    }
}
