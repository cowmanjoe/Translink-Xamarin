﻿using System;
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

            expectedTimeDict.Add("050", expectedTimes);

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
            expectedTimeDict.Add("004", expectedTimes4);

            List<string> expectedTimes7 = new List<string>();
            expectedTimes7.Add("10:39pm");
            expectedTimes7.Add("10:59pm");
            expectedTimes7.Add("11:19pm");
            expectedTimes7.Add("11:39pm");
            expectedTimes7.Add("11:59pm"); 
            expectedTimes7.Add("12:19am");
            expectedTimeDict.Add("007", expectedTimes7);

            List<string> expectedTimes10 = new List<string>();
            expectedTimes10.Add("10:43pm");
            expectedTimes10.Add("11:02pm");
            expectedTimes10.Add("11:22pm");
            expectedTimes10.Add("11:42pm");
            expectedTimes10.Add("12:12am");
            expectedTimeDict.Add("010", expectedTimes10);

            List<string> expectedTimes14 = new List<string>();
            expectedTimes14.Add("10:41pm");
            expectedTimes14.Add("11:06pm");
            expectedTimes14.Add("11:20pm");
            expectedTimes14.Add("11:40pm");
            expectedTimes14.Add("12:10am");
            expectedTimes14.Add("12:25am");
            expectedTimeDict.Add("014", expectedTimes14);

            Assert.IsTrue(AreTimeDictionariesEqual(expectedTimeDict, actualTimeDict)); 
        }

        [TestMethod]
        [ExpectedException(typeof(NoDeparturesFoundException))]
        public void TestParseDepartureTimesNoRoutes()
        {
            
            using (StreamReader sr = new StreamReader(resourcePath + "NoDeparturesFound.xml"))
            {
                DataParser.ParseDepartureTimes(sr.BaseStream);
            }

            Assert.Fail("Expected NoDeparturesFoundException not thrown"); 
        }

        [TestMethod]
        public void TestParseDepartureTimesInvalidStop()
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
        public void TestParseDepartureTimesStopNotFound()
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
        public void TestParseStopInfo()
        {
            StopDataFetcher.StopInfo actualStopInfo;

            using (StreamReader sr = new StreamReader(resourcePath + "Stop55612.xml"))
            {
                actualStopInfo = DataParser.ParseStopInfo(sr.BaseStream);
            }

            StopDataFetcher.StopInfo expectedStopInfo;

            expectedStopInfo.stopNo = 55612;
            expectedStopInfo.name = "SURREY CENTRAL STN BAY 4";
            expectedStopInfo.bayNo = 4;
            expectedStopInfo.onStreet = "SURREY CENTRAL STN";
            expectedStopInfo.atStreet = "BAY 4";
            expectedStopInfo.latitude = 49.188850;
            expectedStopInfo.longitude = -122.849370;
            expectedStopInfo.routes = new List<string>();
            expectedStopInfo.routes.Add("501");
            expectedStopInfo.routes.Add("509");
            expectedStopInfo.routes.Add("N19");

            Assert.AreEqual(expectedStopInfo, actualStopInfo); 
        }

        [TestMethod]
        public void TestParseStopsInfo()
        {
            List<StopDataFetcher.StopInfo> actualStopInfos;
            List<StopDataFetcher.StopInfo> expectedStopInfos = new List<StopDataFetcher.StopInfo>();
            using (StreamReader sr = new StreamReader(resourcePath + "StopSearch1.xml"))
            {
                actualStopInfos = DataParser.ParseStopsInfo(sr.BaseStream);
            }

            StopDataFetcher.StopInfo stop1 = new StopDataFetcher.StopInfo();
            stop1.stopNo = 51516;
            stop1.name = "EB W KING EDWARD AVE FS MANITOBA ST";
            stop1.bayNo = -1;
            stop1.onStreet = "W KING EDWARD AVE";
            stop1.atStreet = "MANITOBA ST";
            stop1.latitude = 49.248820;
            stop1.longitude = -123.107050;
            stop1.routes = new List<string>();
            stop1.routes.Add("025");

            StopDataFetcher.StopInfo stop2 = new StopDataFetcher.StopInfo();
            stop2.stopNo = 51573;
            stop2.name = "WB W KING EDWARD AVE FS COLUMBIA ST";
            stop2.bayNo = -1;
            stop2.onStreet = "W KING EDWARD AVE";
            stop2.atStreet = "COLUMBIA ST";
            stop2.latitude = 49.249020;
            stop2.longitude = -123.110530;
            stop2.routes = new List<string>();
            stop2.routes.Add("025");

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