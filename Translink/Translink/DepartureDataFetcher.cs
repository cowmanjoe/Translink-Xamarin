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
        private const int DEFAULT_DEPARTURE_COUNT = 3;

        private HttpClient httpClient; 
        
        public int DepartureCount
        {
            get; set; 
        }

        public DepartureDataFetcher()
        {
            DepartureCount = DEFAULT_DEPARTURE_COUNT;
            httpClient = new HttpClient(); 
        }

        public async Task<List<Departure>> fetchDepartures(int stop)
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


        public async Task<List<Departure>> fetchDepartures(int stop, int route)
        {
            List<Departure> departures = new List<Departure>();
            string departureData = await fetchDepartureData(stop, route);
            Dictionary<int, List<string>> lts = DataParser.parseDepartureTimes(departureData); 

            
            List<string> times;
            lts.TryGetValue(route, out times); 
            foreach (string time in times) 
                departures.Add(new Departure(time, stop, route)) ;

            return departures; 
        }




        private async Task<string> fetchDepartureData(int stop)
        {

           

            HttpResponseMessage response = await httpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops/" + stop + 
                "/estimates?apikey=" + API_KEY + "&count=" + DepartureCount);

            HttpContent content = response.Content;
            var contentString = await content.ReadAsStringAsync();
            return contentString;
        }


        private async Task<string> fetchDepartureData(int stop, int route)
        {
            HttpResponseMessage response = await httpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops/" + stop + 
                "/estimates?apikey=" + API_KEY + "&count=" + DepartureCount + "&routeNo=" + route);

            HttpContent content = response.Content;
            var contentString = await content.ReadAsStringAsync();
            return contentString; 
        }

    }
}
