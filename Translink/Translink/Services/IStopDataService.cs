using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public interface IStopDataService
    {
        Task<StopInfo> FetchStopInfo(int stop);

        Task<Stop> FetchStopWithDepartures(int stop); 
    }
}
