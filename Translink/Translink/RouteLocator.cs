using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        // ASSUMES: routes StopLocator.FetchStopsAndDeparturesAroundMe returns stops from closest to farthest
        public static async Task<List<Route>> FetchRoutesWithStopsAroundMe(int radius)
        {
            List<Stop> stops = await StopLocator.FetchStopsAndDeparturesAroundMe(radius);

            List<Route> routeList = new List<Route>();
            Dictionary<string, List<string>> routeDirectionsAdded = new Dictionary<string, List<string>>();

            foreach (Stop s in stops)
            {
                foreach (Departure d in s.Departures)
                {
                    Route route = new Route(d.RouteNumber, d.Direction, s);

                    bool duplicateNumberAndDirection = false; 
                    foreach (Route r in routeList)
                    {
                        if (r.Number == route.Number && r.Direction == route.Direction)
                            duplicateNumberAndDirection = true; 
                    }
                    if (!duplicateNumberAndDirection)
                        routeList.Add(route); 
                }
            }

            return routeList;
        }


    }
}
