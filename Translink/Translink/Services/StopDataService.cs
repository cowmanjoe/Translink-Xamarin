using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public class StopDataService : IStopDataService
    {
        private const int DEFAULT_RADIUS = 300; 

        public async Task<StopInfo> FetchStopInfo(int stop)
        {
            return await StopDataFetcher.Instance.FetchStopInfo(stop);
        }

        public async Task<Stop> FetchStopWithDepartures(int stop)
        {
            return await StopDataFetcher.Instance.FetchStopWithDepartures(stop); 
        }

        public async Task<List<StopInfo>> FetchStopInfosAroundMe()
        {
            var position = await Locator.GetPositionAsync();

            return await StopDataFetcher.Instance.SearchStopInfo(position.Latitude, position.Longitude, DEFAULT_RADIUS); 
        }
    }
}
