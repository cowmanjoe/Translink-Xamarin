using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; 

namespace Translink
{
    static class DataParser
    {
        /**
         * Parse the text that the API returns on a departure time request
         * PARAM data: the string received from Translink API 
         * RETURNS: mapping of routes to a list of string representations of departure times 
         **/
        public static Dictionary<string, List<string>> parseDepartureTimes(string data)
        {
            Dictionary<string, List<string>> ans = new Dictionary<string, List<string>>();
            string dataSoFar = data;

            Debug.WriteLine("Parsing this: " + data); 


            // while there are still more routes in the data
            while (dataSoFar.Contains("<RouteNo>"))
            {
                dataSoFar = dataSoFar.Substring(dataSoFar.IndexOf("<RouteNo>", StringComparison.Ordinal) + "<RouteNo>".Length);
                string route = dataSoFar.Substring(0, dataSoFar.IndexOf('<'));
                
                List<string> times = new List<string>();

                dataSoFar = dataSoFar.Substring(dataSoFar.IndexOf("</RouteNo>", StringComparison.Ordinal) + "</RouteNo>".Length); 

                // while there are still departure times in the current route
                while (dataSoFar.Contains("<ExpectedLeaveTime>") &&
                    (dataSoFar.IndexOf("<ExpectedLeaveTime>", StringComparison.Ordinal) < dataSoFar.IndexOf("<RouteNo>", StringComparison.Ordinal) || 
                    !dataSoFar.Contains("RouteNo>")))
                {
                    int index = dataSoFar.IndexOf("<ExpectedLeaveTime>", StringComparison.Ordinal) + "<ExpectedLeaveTime>".Length;
                    dataSoFar = dataSoFar.Substring(index);
                    string time = dataSoFar.Substring(0, dataSoFar.IndexOf('m') + 1);
                    times.Add(time); 
                }
                ans.Add(route, times); 
            }
            foreach (string r in ans.Keys)
            {
                Debug.WriteLine("Route #" + r + " being returned"); 
            }

            return ans; 
        }
    }
}
