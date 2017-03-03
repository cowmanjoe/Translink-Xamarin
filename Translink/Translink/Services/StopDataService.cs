using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public class StopDataService : IStopDataService
    {
        public async Task<StopInfo> FetchStopInfo(int stop)
        {
            return await StopDataFetcher.Instance.FetchStopInfo(stop);
        }

        public async Task<Stop> FetchStopWithDepartures(int stop)
        {
            return await StopDataFetcher.Instance.FetchStopWithDepartures(stop); 
        }
    }
}
