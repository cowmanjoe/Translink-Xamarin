using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Models;

namespace Translink
{
    public static class RouteLocator
    {
        public static async Task<List<string>> FetchRouteNumbersAroundMe(int radius)
        { 
            List<StopInfo> stopInfos = await StopLocator.FetchStopsAroundMe(radius); 

            List<string> routes = new List<string>(); 
            foreach (StopInfo si in stopInfos)
            {
                foreach (string r in si.routes)
                {
                    if (!routes.Contains(r))
                        routes.Add(r); 
                }
            }
            return routes; 
        }


        public static async Task<List<Route>> FetchRoutesWithStopsAroundMe(int radius)
        {
            List<StopInfo> stopInfos = await StopLocator.FetchStopsAroundMe(radius);

            List<Route> routeList = new List<Route>(); 

             
            foreach (StopInfo si in stopInfos)
            {
                foreach (string r in si.routes)
                {
                    Route route = Route.GetRouteWithNumber(r, routeList); 
                    if (route != null)
                    {
                        route.AddStop(si.stopNo); 
                    }
                    else
                    {
                        route = new Route(r);
                        route.AddStop(si.stopNo);
                        routeList.Add(route);
                    }
                    
                }
            }

            return routeList; 
        }

    }
}
