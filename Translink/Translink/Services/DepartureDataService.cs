using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Exception;
using Translink.Services;

namespace Translink.Services
{
    public class DepartureDataService : IDepartureDataService
    {
        #region IDepartureDataService implementation

        /**
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         */
        public async Task<List<Departure>> SearchDepartures(int stopNo)
        {
            return await DepartureDataFetcher.Instance.FetchDepartures(stopNo);
        }

        /** 
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         * route: the route number 
         */ 
        public async Task<List<Departure>> SearchDepartures(int stopNo, string routeNo)
        {
            return await DepartureDataFetcher.Instance.FetchDepartures(stopNo, routeNo); 
        }

        #endregion
    }
}
