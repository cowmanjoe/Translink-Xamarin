using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    class DataParser
    {
        /**
         * Parse the text that the API returns on a departure time request
         * @param data the string received from Translink API 
         * @return mapping of routes to a list of string representations of departure times 
         **/
        public static Dictionary<int, List<String>> parseDepartureTimes(string data)
        {
            Dictionary<int, List<String>> ans = new Dictionary<int, List<String>>();
            string dataSoFar = data; 

            // while there are still more routes in the data
            while (dataSoFar.Contains("<RouteNo>"))
            {
                dataSoFar = dataSoFar.Substring(dataSoFar.IndexOf("<RouteNo>") + "<RouteNo>".Length);
                string routeString = dataSoFar.Substring(0, dataSoFar.IndexOf('<'));
                int route = Convert.ToInt32(routeString);
                List<string> times = new List<string>();

                dataSoFar = dataSoFar.Substring(dataSoFar.IndexOf("</RouteNo>") + "</RouteNo>".Length); 

                // while there are still departure times in the current route
                while (dataSoFar.Contains("<ExpectedLeaveTime>") &&
                    (dataSoFar.IndexOf("<ExpectedLeaveTime>") < dataSoFar.IndexOf("<RouteNo>") || 
                    !dataSoFar.Contains("RouteNo>")))
                {
                    int index = dataSoFar.IndexOf("<ExpectedLeaveTime>") + "<ExpectedLeaveTime>".Length;
                    dataSoFar = dataSoFar.Substring(index);
                    string time = dataSoFar.Substring(0, dataSoFar.IndexOf('m') + 1);
                    times.Add(time); 
                }
                ans.Add(route, times); 
            }
            return ans; 
        }
    }
}
