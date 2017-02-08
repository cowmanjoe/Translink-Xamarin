using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public static class RouteLocator
    {
        public static async Task<List<string>> FetchRoutesAroundMe(int radius)
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


    }
}
