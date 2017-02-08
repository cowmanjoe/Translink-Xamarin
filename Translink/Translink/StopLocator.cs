using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Translink
{
    public static class StopLocator
    {
        public static async Task<List<StopInfo>> FetchStopsAroundMe(int radius)
        {
            if (radius < 0 || radius > 2000)
                throw new ArgumentOutOfRangeException("Radius must be between 0 and 2000");

            
            var position = await Locator.getPositionAsync(); 

            double latitude = position.Latitude;
            double longitude = position.Longitude;

            StopDataFetcher sdf = StopDataFetcher.Instance;

            return await sdf.SearchStopInfo(latitude, longitude, radius); 
        }


        public static async Task<List<Stop>> FetchStopsAndDeparturesAroundMe(int radius)
        {
            if (radius < 0 || radius > 2000)
                throw new ArgumentOutOfRangeException("Radius must be between 0 and 2000");

            var position = await Locator.getPositionAsync(); 

            double latitude = position.Latitude;
            double longitude = position.Longitude;

            StopDataFetcher stopDataFetcher = StopDataFetcher.Instance;

            return await stopDataFetcher.SearchStopsWithDepartures(latitude, longitude, radius); 
        }
    }
}
