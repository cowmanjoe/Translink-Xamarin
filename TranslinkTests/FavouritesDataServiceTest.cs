using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Translink;
using System.Collections.Generic;
using Translink.Models;
using Translink.Services;
using RouteDirection = System.Tuple<string, string>; 

namespace TranslinkTests
{
    [TestClass]
    public class FavouritesDataServiceTest
    {
        const string resourcePath = "F:/Cowan/Documents/Visual Studio 2015/Projects/Translink/TranslinkTests/Resources/";

        [TestMethod]
        public void ParseFavouriteStopInfos_Empty()
        {
            FavouritesDataService dataService = new FavouritesDataService();
            List<StopInfo> actualStopInfos;
            using (StreamReader sr = new StreamReader(resourcePath + "NoStopFavourites.xml"))
            {
                actualStopInfos = dataService.ParseFavouriteStopInfos(sr.BaseStream);
            }
            Assert.AreEqual(0, actualStopInfos.Count);
        }

        [TestMethod]
        public void ParseFavouriteStopInfos_3Entries()
        {
            FavouritesDataService dataService = new FavouritesDataService();
            List<StopInfo> actualStopInfos;
            using (StreamReader sr = new StreamReader(resourcePath + "Favourites1.xml"))
            {
                actualStopInfos = dataService.ParseFavouriteStopInfos(sr.BaseStream);
            }

            List<StopInfo> expectedStopInfos = new List<StopInfo>();
            StopInfo s1 = new StopInfo(50586, "WB W 4 AVE FS BAYSWATER STN");
            StopInfo s2 = new StopInfo(60980, "SB GRANVILLE ST NS W GEORGIA STN");
            StopInfo s3 = new StopInfo(50585, "WB W 4 AVE FS MACDONALD STN");
            expectedStopInfos.Add(s1);
            expectedStopInfos.Add(s2);
            expectedStopInfos.Add(s3);

            Assert.IsTrue(ListEquals<StopInfo>(expectedStopInfos, actualStopInfos)); 
        }

        [TestMethod]
        public void ParseFavouriteRouteNumbers_Empty()
        {
            FavouritesDataService dataService = new FavouritesDataService();
            List<RouteDirection> actualRoutes;
            using (StreamReader sr = new StreamReader(resourcePath + "NoRouteFavourites.xml"))
            {
                actualRoutes = dataService.ParseFavouriteRouteDirections(sr.BaseStream); 
            }

            Assert.AreEqual(0, actualRoutes.Count); 
        }

        [TestMethod]
        public void ParseFavouriteRouteNumbers_2Entries()
        {
            FavouritesDataService dataService = new FavouritesDataService();
            List<RouteDirection> actualRoutes;
            using (StreamReader sr = new StreamReader(resourcePath + "Favourites1.xml"))
            {
                actualRoutes = dataService.ParseFavouriteRouteDirections(sr.BaseStream);
            }

            List<RouteDirection> expectedRoutes = new List<RouteDirection>();
            expectedRoutes.Add(new RouteDirection("004", "WEST"));
            expectedRoutes.Add(new RouteDirection("C18", "SOUTH")); 

            Assert.IsTrue(ListEquals(expectedRoutes, actualRoutes)); 
        }

        private bool ListEquals<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false; 
            }
            return true; 
        }
    }
}
