using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Translink;
using Translink.Exception; 
using System.Collections.Generic;
using System.Linq;
using Translink.Models;
using RouteDirection = System.Tuple<string, string>;

namespace TranslinkTests
{ 
    [TestClass]
    public class DataParserTest
    {
        const string resourcePath = "F:/Cowan/Documents/Visual Studio 2015/Projects/Translink/TranslinkTests/Resources/"; 

        [TestMethod]
        public void TestParseDepartureTimesSingleRoute()
        {
            Dictionary<RouteDirection, List<DateTime>> actualTimeDict;
            using (StreamReader sr = new StreamReader(resourcePath + "Departures60980_50.xml"))
            {
                actualTimeDict = DataParser.ParseDepartureTimes(sr.BaseStream);
            }

            Dictionary<RouteDirection, List<DateTime>> expectedTimeDict = new Dictionary<RouteDirection, List<DateTime>>();
            List<DateTime> expectedTimes = new List<DateTime>
            {
                DateTime.Now.AddMinutes(21),
                DateTime.Now.AddMinutes(51),
                DateTime.Now.AddMinutes(81),
                DateTime.Now.AddMinutes(112)
            };

            expectedTimeDict.Add(new RouteDirection("050", "SOUTH"), expectedTimes);

            Assert.IsTrue(AreTimeDictionariesEqual(expectedTimeDict, actualTimeDict)); 
        }

        [TestMethod]
        public void TestParseDepartureTimesManyRoutes()
        {
            Dictionary<RouteDirection, List<DateTime>> actualTimeDict;
            using (StreamReader sr = new StreamReader(resourcePath + "Departures60980.xml"))
            {
                actualTimeDict = DataParser.ParseDepartureTimes(sr.BaseStream); 
            }

            Dictionary<RouteDirection, List<DateTime>> expectedTimeDict = new Dictionary<RouteDirection, List<DateTime>>();
            List<DateTime> expectedTimes4 = new List<DateTime>
            {
                DateTime.Now.AddMinutes(10),
                DateTime.Now.AddMinutes(30),
                DateTime.Now.AddMinutes(50),
                DateTime.Now.AddMinutes(70),
                DateTime.Now.AddMinutes(90)
            };
            expectedTimeDict.Add(new RouteDirection("004", "WEST"), expectedTimes4);


            List<DateTime> expectedTimes7 = new List<DateTime>
            {
                DateTime.Now.AddMinutes(0),
                DateTime.Now.AddMinutes(20),
                DateTime.Now.AddMinutes(40),
                DateTime.Now.AddMinutes(60),
                DateTime.Now.AddMinutes(80),
                DateTime.Now.AddMinutes(100)
            };
            expectedTimeDict.Add(new RouteDirection("007", "WEST"), expectedTimes7);

            List<DateTime> expectedTimes10 = new List<DateTime>
            {
                DateTime.Now.AddMinutes(4),
                DateTime.Now.AddMinutes(23),
                DateTime.Now.AddMinutes(43),
                DateTime.Now.AddMinutes(63),
                DateTime.Now.AddMinutes(93)
            };
            expectedTimeDict.Add(new RouteDirection("010", "SOUTH"), expectedTimes10);

            List<DateTime> expectedTimes14 = new List<DateTime>
            {
                DateTime.Now.AddMinutes(2),
                DateTime.Now.AddMinutes(27),
                DateTime.Now.AddMinutes(41),
                DateTime.Now.AddMinutes(61),
                DateTime.Now.AddMinutes(91),
                DateTime.Now.AddMinutes(106)
            };
            expectedTimeDict.Add(new RouteDirection("014", "WEST"), expectedTimes14);
            
            Assert.IsTrue(AreTimeDictionariesEqual(expectedTimeDict, actualTimeDict)); 
        }

        [TestMethod]
        public void TestParseDepartureTimes_InvalidStop_ThrowException()
        {
            try
            {
                using (StreamReader sr = new StreamReader(resourcePath + "InvalidStopNumber.xml"))
                {
                    DataParser.ParseDepartureTimes(sr.BaseStream); 
                }

                Assert.Fail("Expected InvalidStopException not thrown"); 
            }
            catch (InvalidStopException ex)
            {
                Assert.IsTrue(ex.Message.Equals("Stop must be a valid 5 digit number."));
            }
        }

        [TestMethod]
        public void TestParseDepartureTimes_StopNotFound_ThrowException()
        {
            try
            {
                using (StreamReader sr = new StreamReader(resourcePath + "StopNumberNotFound.xml"))
                {
                    DataParser.ParseDepartureTimes(sr.BaseStream);
                }

                Assert.Fail("Expected InvalidStopException not thrown");
            }
            catch (InvalidStopException ex)
            {
                Assert.IsTrue(ex.Message.Equals("Stop not found."));
            }
        }

        [TestMethod]
        public void TestParseStopInfo_56612()
        {
            StopInfo actualStopInfo;

            using (StreamReader sr = new StreamReader(resourcePath + "Stop55612.xml"))
            {
                actualStopInfo = DataParser.ParseStopInfo(sr.BaseStream);
            }

            int number = 55612;
            string name = "SURREY CENTRAL STN BAY 4";
            StopInfo expectedStopInfo = new StopInfo(number, name);
            
            expectedStopInfo.BayNumber = 4;
            expectedStopInfo.OnStreet = "SURREY CENTRAL STN";
            expectedStopInfo.AtStreet = "BAY 4";
            expectedStopInfo.Latitude = 49.188850;
            expectedStopInfo.Longitude = -122.849370;
            expectedStopInfo.Routes = new List<string>();
            expectedStopInfo.Routes.Add("501");
            expectedStopInfo.Routes.Add("509");
            expectedStopInfo.Routes.Add("N19");

            Assert.AreEqual(expectedStopInfo, actualStopInfo); 
        }

        [TestMethod]
        public void TestParseStopsInfo_StopSearch1()
        {
            List<StopInfo> actualStopInfos;
            List<StopInfo> expectedStopInfos = new List<StopInfo>();
            using (StreamReader sr = new StreamReader(resourcePath + "StopSearch1.xml"))
            {
                actualStopInfos = DataParser.ParseStopsInfo(sr.BaseStream);
            }
            
            StopInfo stop1 = new StopInfo(51516, "EB W KING EDWARD AVE FS MANITOBA ST"); 
            stop1.BayNumber = -1;
            stop1.OnStreet = "W KING EDWARD AVE";
            stop1.AtStreet = "MANITOBA ST";
            stop1.Latitude = 49.248820;
            stop1.Longitude = -123.107050;
            stop1.Routes = new List<string>();
            stop1.Routes.Add("025");

            StopInfo stop2 = new StopInfo(51573, "WB W KING EDWARD AVE FS COLUMBIA ST");
            stop2.BayNumber = -1;
            stop2.OnStreet = "W KING EDWARD AVE";
            stop2.AtStreet = "COLUMBIA ST";
            stop2.Latitude = 49.249020;
            stop2.Longitude = -123.110530;
            stop2.Routes = new List<string>();
            stop2.Routes.Add("025");

            expectedStopInfos.Add(stop1);
            expectedStopInfos.Add(stop2);

            Assert.AreEqual(expectedStopInfos.Count, actualStopInfos.Count);
            for (int i = 0; i < expectedStopInfos.Count; i++)
            {
                Assert.IsTrue(expectedStopInfos[i].Equals(actualStopInfos[i]));
            }
        }
        

        private bool AreTimeDictionariesEqual(Dictionary<RouteDirection, List<DateTime>> x, Dictionary<RouteDirection, List<DateTime>> y)
        {
            if (x.Count != y.Count)
                return false;
            if (x.Keys.AsEnumerable().Except(y.Keys).Any())
                return false;
            if (y.Keys.AsEnumerable().Except(x.Keys).Any())
                return false;
            foreach (KeyValuePair<RouteDirection, List<DateTime>> pair in x)
            {
                if (AreListsEqual(pair.Value, y[pair.Key])) continue;
                return false;
            }
            return true;
        }

        private bool AreListsEqual(List<DateTime> l1, List<DateTime> l2)
        {
            if (l1.Count != l2.Count)
                return false;
            for (int i = 0; i < l1.Count; i++)
            {
                if (AreTimesWithin(l1[i], l2[i], 10)) continue;
                return false;
            }
            return true;
        }

        private bool AreTimesWithin(DateTime t1, DateTime t2, int seconds)
        {
            return (seconds >= Math.Abs(t1.Subtract(t2).TotalSeconds));
        }
        
    }
}
