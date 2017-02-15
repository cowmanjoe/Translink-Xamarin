using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Translink.Services
{
    public class RouteDataService : IRouteDataService
    {
        private const int DEFAULT_RADIUS = 500;

        public async Task<List<Route>> GetRoutes()
        {
            return await RouteLocator.FetchRoutesWithStopsAroundMe(DEFAULT_RADIUS); 
        }
    }
}
