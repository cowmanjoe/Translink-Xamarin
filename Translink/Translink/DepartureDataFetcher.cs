using System.Diagnostics; 
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Translink
{
    // Class containing an HttpClient for fetching the actual Translink API data from the internet 
    class DepartureDataFetcher 
    {
        private const string API_KEY = "gmSNagac7UUqdirk3bFj";
        private const int DEFAULT_DEPARTURE_COUNT = 3;

        private readonly HttpClient mHttpClient; 
        

        // Number of departures (per route) that will be fetched 
        public int DepartureCount
        {
            get; set; 
        }


        public DepartureDataFetcher()
        {
            DepartureCount = DEFAULT_DEPARTURE_COUNT;
            mHttpClient = new HttpClient();
            
        }

        /**
         * Fethes asynchronously the next DepartureCount departures at the given stop 
         * stop: is the 5 digit ID of the stop 
         * RETURNS: the correct list of departures 
         */
        public async Task<List<Departure>> fetchDepartures(int stop)
        {
            List<Departure> departures = new List<Departure>();
            Stream departureStream = await fetchDepartureData(stop); 
            Dictionary<string, List<string>> lts = DataParser.parseDepartureTimes(departureStream); 

            foreach (string route in lts.Keys)
            {
                List<string> times;
                lts.TryGetValue(route, out times);
                foreach (string time in times)
                    departures.Add(new Departure(time, stop, route)); 
            }
            Debug.WriteLine("Returning departures!");
            return departures;  
        }

        /**
         * Fetches asynchronously the next DepartureCount departures at the given stop and route
         * CAREFUL route may not always be the same as the returned route, 
         * e.g. argument route = "10" but returned route is "010" 
         * stop: the 5 digit ID of the stop 
         * route: the route number (usually 1-3 digits) 
         * RETURNS: the retrieved list of departures (empty list if none) 
         */
        public async Task<List<Departure>> fetchDepartures(int stop, string route)
        {
            List<Departure> departures = new List<Departure>();
            Debug.WriteLine("Before"); 
            Stream departureStream = await FetchDepartureData(stop, route);
            Dictionary<string, List<string>> lts = DataParser.parseDepartureTimes(departureStream);
            Debug.WriteLine("Departure data parsed!");
            
            foreach (string r in lts.Keys)
            {
                List<string> times;
                lts.TryGetValue(r, out times); 
                Debug.WriteLine("Route value found!");
                foreach (string time in times)
                    departures.Add(new Departure(time, stop, r));
            }
            Debug.WriteLine("Returning departures!"); 
            return departures; 
        }

        

        /**
         * Fetches the raw data from the Translink API for departures at the given stop 
         * stop: is the 5 digit ID of the stop 
         * RETURNS: the string with the raw XML data for the departures 
         */
        private async Task<Stream> fetchDepartureData(int stop)
        {
            string uri = "http://api.translink.ca/rttiapi/v1/stops/" + stop +
                "/estimates?apikey=" + API_KEY + "&count=" + DepartureCount;

            Debug.WriteLine("URI: " + uri); 

            HttpResponseMessage response = await mHttpClient.GetAsync(uri);
           
            HttpContent content = response.Content;
            var contentStream = await content.ReadAsStreamAsync();
            return contentStream;
        }

        /**
         * Fetches the raw data from the Translink API for departures at the given stop 
         * stop: the 5 digit ID of the stop 
         * route: the route number (usually 1-3 digits) 
         * RETURNS: the string with the raw XML data for the departures 
         */
        private async Task<Stream> FetchDepartureData(int stop, string route)
        {
            HttpResponseMessage response = await mHttpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops/" + stop + 
                "/estimates?apikey=" + API_KEY + "&count=" + DepartureCount + "&routeNo=" + route);

            HttpContent content = response.Content;
            var contentStream = await content.ReadAsStreamAsync();
            return contentStream; 
        }

    }
}
