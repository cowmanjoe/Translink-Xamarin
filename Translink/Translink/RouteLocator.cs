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
                foreach (string d in s.Departures)
                {
                    Route route = Route.GetRouteWithNumber(d.RouteNumber, routeList);
                    List<string> directions;
                    if (route == null)
                    {
                        route = new Route(d.RouteNumber);
                        route.AddStop(s.Number);
                        routeList.Add(route);

                        directions = new List<string>();
                        directions.Add(d.Direction);
                        routeDirectionsAdded.Add(d.RouteNumber, directions);
                    }
                    else if (routeDirectionsAdded.TryGetValue(d.RouteNumber, out directions) &&
                        !directions.Contains(d.Direction))
                    {
                        route.AddStop(s.Number);
                    }
                }
            }

            return routeList;
        }


    }
}
