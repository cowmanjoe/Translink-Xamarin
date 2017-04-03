using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using Translink.Exception;
using System;
using Translink.Models;
using RouteDirection = System.Tuple<string, string>;

namespace Translink
{
    // Class containing an HttpClient for fetching the actual Translink API data from the internet
    public class DepartureDataFetcher
    {
        private const string API_KEY = "gmSNagac7UUqdirk3bFj";
        private const int DEFAULT_DEPARTURE_COUNT = 3;

        private readonly HttpClient mHttpClient;

        private static DepartureDataFetcher mInstance;


        // Number of departures (per route) that will be fetched
        public int DepartureCount
        {
            get; set;
        }

        public static DepartureDataFetcher Instance => mInstance ?? (mInstance = new DepartureDataFetcher());


        private DepartureDataFetcher()
        {
            DepartureCount = DEFAULT_DEPARTURE_COUNT;
            mHttpClient = new HttpClient();

        }

        public async Task<List<Departure>> FetchDepartures(int stopNo)
        {
            List<Departure> departures = new List<Departure>();
            Stream departureStream = await fetchDepartureData(stopNo);

            Dictionary<RouteDirection, List<DateTime>> lts = DataParser.ParseDepartureTimes(departureStream);

            foreach (RouteDirection routeDir in lts.Keys)
            {
                List<DateTime> times;
                lts.TryGetValue(routeDir, out times);
                foreach (DateTime time in times)
                    departures.Add(new Departure(time, stopNo, routeDir.Item1, routeDir.Item2));
            }
            return departures;
        }

        public async Task<List<Departure>> FetchDepartures(int stopNo, string routeNo)
        {
            
            List<Departure> departures = new List<Departure>();
            Stream departureStream = await FetchDepartureData(stopNo, routeNo);
            Dictionary<RouteDirection, List<DateTime>> lts = DataParser.ParseDepartureTimes(departureStream);

            foreach (RouteDirection routeDir in lts.Keys)
            {
                List<DateTime> times;
                lts.TryGetValue(routeDir, out times);
                foreach (DateTime time in times)
                    departures.Add(new Departure(time, stopNo, routeDir.Item1, routeDir.Item2));
            }
            return departures;
        }

        /**
         * Fethes asynchronously the next DepartureCount departures at the given stop
         * stop: is the 5 digit ID of the stop
         * RETURNS: the correct list of departures
         */
        public async Task<List<Departure>> FetchDepartures(Stop stop)
        {
            List<Departure> departures = new List<Departure>();
            Stream departureStream = await fetchDepartureData(stop.Number);
            Dictionary<RouteDirection, List<DateTime>> lts = DataParser.ParseDepartureTimes(departureStream);

            foreach (RouteDirection routeDir in lts.Keys)
            {
                List<DateTime> times;
                lts.TryGetValue(routeDir, out times);
                foreach (DateTime time in times)
                    departures.Add(new Departure(time, stop, routeDir.Item1, routeDir.Item2));
            }
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
        public async Task<List<Departure>> fetchDepartures(Stop stop, string route)
        {
            bool stopHasRoute = false;
            foreach (string r in stop.Routes) {
                if (Util.RouteEquals(r, route))
                    stopHasRoute = true;
            }
            if (!stopHasRoute)
                throw new ArgumentException("The stop " + stop.Number + "does not have a route " + route);

            List<Departure> departures = new List<Departure>();
            Stream departureStream = await FetchDepartureData(stop.Number, route);
            Dictionary<RouteDirection, List<DateTime>> lts = DataParser.ParseDepartureTimes(departureStream);

            foreach (RouteDirection routeDir in lts.Keys)
            {
                List<DateTime> times;
                lts.TryGetValue(routeDir, out times);
                foreach (DateTime time in times)
                    departures.Add(new Departure(time, stop, routeDir.Item1, routeDir.Item2));
            }
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

            HttpResponseMessage response = await mHttpClient.GetAsync(uri);

            HttpContent content = response.Content;
            Stream contentStream = await content.ReadAsStreamAsync();
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
            Stream contentStream = await content.ReadAsStreamAsync();
            return contentStream;
        }

    }
}
