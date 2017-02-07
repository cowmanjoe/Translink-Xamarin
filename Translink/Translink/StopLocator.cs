using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
namespace Translink
{
    public static class StopLocator
    {

        public static async Task<List<Stop>> FetchStopsAroundMe(int radius)
        {
            if (radius < 0 || radius > 2000)
                throw new ArgumentOutOfRangeException("Radius must be between 0 and 2000"); 

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();

            double longitude = Math.Round(position.Longitude, 6);
            double latitude = Math.Round(position.Latitude, 6);

            StopDataFetcher stopDataFetcher = StopDataFetcher.Instance;

            return await stopDataFetcher.SearchStopsWithDepartures(latitude, longitude, radius); 
        }
    }
}
