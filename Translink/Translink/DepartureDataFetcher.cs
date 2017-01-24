using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Translink
{
    class DepartureDataFetcher
    {
        private const string API_KEY = "gmSNagac7UUqdirk3bFj"; 

        public static async Task<List<Departure>> getDepartures(int stop)
        {
            List<Departure> departures = new List<Departure>();
            string departureData = await fetchDepartureData(stop); 
            Dictionary<int, List<string>> lts = DataParser.parseDepartureTimes(departureData); 

            foreach (int route in lts.Keys)
            {
                List<string> times;
                lts.TryGetValue(route, out times);
                foreach (string time in times)
                    departures.Add(new Departure(time, stop, route)); 
            }

            return departures; 
        }




        private static async Task<string> fetchDepartureData(int stop)
        {
            var client = new HttpClient(); 
            


            HttpResponseMessage response = await client.GetAsync("http://api.translink.ca/rttiapi/v1/stops/" + stop + "/estimates?apikey=" + API_KEY);
            
            HttpContent content = response.Content;
            var contentString = await content.ReadAsStringAsync(); 
            return contentString; 
        }

        /** Generates URL for departure times for all routes from a particular stop 
         * @param stop the stop number 
         * @return URL or null if a MalformedURLException is thrown 
         **/
         private static Uri getUriForDepartures(int stop)
        {
           
            string uri = "http://api.translink.ca/rttiapi/v1/stops/" + stop + "/estimates?apikey=" + API_KEY;
            return new Uri(uri); 
           
        }

    }
}
