using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public class Route
    {
        public string Number
        {
            get; 
        }

        private readonly HashSet<int> mStops;
        

        public Route(string number)
        {
            Number = number;
            mStops = new HashSet<int>(); 
        }

        

        public string StopsAsString
        {
            get
            {
                StringBuilder stops = new StringBuilder();
                foreach (int stop in mStops)
                {
                    stops.Append(stop);
                    stops.Append(" "); 
                }
                return stops.ToString();
            }
        }

        
        public Route(string number, HashSet<int> stops)
        {
            Number = number;
            mStops = stops;
        }

        public void AddStop(int stop)
        {
            mStops.Add(stop); 
        }

        public static Route GetRouteWithNumber(string number, IEnumerable<Route> routes)
        {
            foreach (Route route in routes)
            {
                if (route.Number == number)
                {
                    return route; 
                }
            }

            return null; 
        }
    }
}
