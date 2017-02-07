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
            if (radius > 2000)
                throw new ArgumentOutOfRangeException("Radius must be greater than 0 and at most 2000"); 

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();

            double longitude = Math.Round(position.Longitude, 6);
            double latitude = Math.Round(position.Latitude, 6);

            StopDataFetcher stopDataFetcher = StopDataFetcher.getInstance();

            return await stopDataFetcher.SearchStopsWithDepartures(latitude, longitude, radius); 
        }
    }
}
