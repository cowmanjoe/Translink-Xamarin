using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Translink;
using System.Collections.Generic;
using Translink.Services;

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
            StopInfo s1 = new StopInfo { Number = 50586, Name = "WB W 4 AVE FS BAYSWATER STN" };
            StopInfo s2 = new StopInfo { Number = 60980, Name = "SB GRANVILLE ST NS W GEORGIA STN" };
            StopInfo s3 = new StopInfo { Number = 50585, Name = "WB W 4 AVE FS MACDONALD STN" };
            expectedStopInfos.Add(s1);
            expectedStopInfos.Add(s2);
            expectedStopInfos.Add(s3);

            Assert.IsTrue(ListEquals<StopInfo>(expectedStopInfos, actualStopInfos)); 
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
