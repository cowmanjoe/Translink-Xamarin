using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Translink;
using Translink.Exception; 
using System.Collections.Generic;
using System.Linq;

namespace TranslinkTests
{ 
    [TestClass]
    public class DataParserTest
    {
        const string resourcePath = "F:/Cowan/Documents/Visual Studio 2015/Projects/Translink/TranslinkTests/Resources/"; 

        [TestMethod]
        public void TestParseDepartureTimesSingleRoute()
        {
            Dictionary<string, List<string>> actualTimeDict;
            using (StreamReader sr = new StreamReader(resourcePath + "Departures60980_50.xml"))
            {
                actualTimeDict = DataParser.ParseDepartureTimes(sr.BaseStream);
            }

            Dictionary<string, List<string>> expectedTimeDict = new Dictionary<string, List<string>>();
            List<string> expectedTimes = new List<string>();
            expectedTimes.Add("10:43pm");
            expectedTimes.Add("11:13pm");
            expectedTimes.Add("11:43pm");
            expectedTimes.Add("12:14am");

            expectedTimeDict.Add("050:SOUTH", expectedTimes);

            Assert.IsTrue(AreTimeDictionariesEqual(expectedTimeDict, actualTimeDict)); 
        }

        [TestMethod]
        public void TestParseDepartureTimesManyRoutes()
        {
            Dictionary<string, List<string>> actualTimeDict;
            using (StreamReader sr = new StreamReader(resourcePath + "Departures60980.xml"))
            {
                actualTimeDict = DataParser.ParseDepartureTimes(sr.BaseStream); 
            }

            Dictionary<string, List<string>> expectedTimeDict = new Dictionary<string, List<string>>();
            List<string> expectedTimes4 = new List<string>();
            expectedTimes4.Add("10:49pm");
            expectedTimes4.Add("11:09pm");
            expectedTimes4.Add("11:29pm");
            expectedTimes4.Add("11:49pm");
            expectedTimes4.Add("12:09am");
            expectedTimeDict.Add("004:WEST", expectedTimes4);

            List<string> expectedTimes7 = new List<string>();
            expectedTimes7.Add("10:39pm");
            expectedTimes7.Add("10:59pm");
            expectedTimes7.Add("11:19pm");
            expectedTimes7.Add("11:39pm");
            expectedTimes7.Add("11:59pm"); 
            expectedTimes7.Add("12:19am");
            expectedTimeDict.Add("007:WEST", expectedTimes7);

            List<string> expectedTimes10 = new List<string>();
            expectedTimes10.Add("10:43pm");
            expectedTimes10.Add("11:02pm");
            expectedTimes10.Add("11:22pm");
            expectedTimes10.Add("11:42pm");
            expectedTimes10.Add("12:12am");
            expectedTimeDict.Add("010:SOUTH", expectedTimes10);

            List<string> expectedTimes14 = new List<string>();
            expectedTimes14.Add("10:41pm");
            expectedTimes14.Add("11:06pm");
            expectedTimes14.Add("11:20pm");
            expectedTimes14.Add("11:40pm");
            expectedTimes14.Add("12:10am");
            expectedTimes14.Add("12:25am");
            expectedTimeDict.Add("014:WEST", expectedTimes14);

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

            StopInfo expectedStopInfo = new StopInfo();

            expectedStopInfo.Number = 55612;
            expectedStopInfo.Name = "SURREY CENTRAL STN BAY 4";
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

            StopInfo stop1 = new StopInfo();
            stop1.Number = 51516;
            stop1.Name = "EB W KING EDWARD AVE FS MANITOBA ST";
            stop1.BayNumber = -1;
            stop1.OnStreet = "W KING EDWARD AVE";
            stop1.AtStreet = "MANITOBA ST";
            stop1.Latitude = 49.248820;
            stop1.Longitude = -123.107050;
            stop1.Routes = new List<string>();
            stop1.Routes.Add("025");

            StopInfo stop2 = new StopInfo();
            stop2.Number = 51573;
            stop2.Name = "WB W KING EDWARD AVE FS COLUMBIA ST";
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

        private bool AreTimeDictionariesEqual(Dictionary<string, List<string>> x, Dictionary<string, List<string>> y)
        {
            if (x.Count != y.Count)
                return false;
            if (x.Keys.AsEnumerable().Except(y.Keys).Any())
                return false;
            if (y.Keys.AsEnumerable().Except(x.Keys).Any())
                return false; 
            foreach (var pair in x)
            {
                if (!AreStringListsEqual(pair.Value, y[pair.Key]))
                    return false; 
            }
            return true; 
        }

        private bool AreStringListsEqual(List<string> x, List<string> y)
        {
            if (x.Count != y.Count)
                return false; 
            for (int i = 0; i < x.Count; i++)
            {
                if (!x[i].Equals(y[i]))
                    return false;
            }
            return true; 
        }
    }
}
