using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Translink.Services
{
    public class RouteDataService : IRouteDataService
    {
        private const int DEFAULT_RADIUS = 300;

        public async Task<Route> GetNearestRoute(string number, string direction)
        {
            List<Route> routes = await GetRoutes(); 
            foreach (Route r in routes)
            {
                if (r.Direction == direction && r.Number == number)
                    return r; 
            }
            return null;
        }

        public async Task<List<Route>> GetRoutes()
        {
            return await RouteLocator.FetchRoutesWithStopsAroundMe(DEFAULT_RADIUS); 
        }
    }
}
