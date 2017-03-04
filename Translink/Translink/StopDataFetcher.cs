using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics; 

namespace Translink
{
    public class StopDataFetcher
    {
        private const string API_KEY = "gmSNagac7UUqdirk3bFj";
        private readonly HttpClient mHttpClient;

        private static StopDataFetcher mInstance; 
        


        private StopDataFetcher()
        {
            mHttpClient = new HttpClient(); 
        }

        public static StopDataFetcher Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new StopDataFetcher();
                return mInstance;
            }
            
        }

        public async Task<Stop> FetchStopWithDepartures(int stopNo)
        {
            StopInfo stopInfo = await (FetchStopInfo(stopNo));
            Stop stop = new Stop(stopInfo); 

            DepartureDataFetcher departureDataFetcher = DepartureDataFetcher.Instance; 
            List<Departure> departures = await departureDataFetcher.FetchDepartures(stop);
            stop.Departures = departures;
            return stop; 
        }

        public async Task<List<Stop>> SearchStopsWithDepartures(double lat, double lon, int radius) 
        {
            List<Stop> stops = new List<Stop>();
            DepartureDataFetcher departureDataFetcher = DepartureDataFetcher.Instance; 
           

            List<StopInfo> stopInfos = await SearchStopInfo(lat, lon, radius); 

            foreach (StopInfo si in stopInfos)
            {
                Stop stop = new Stop(si); 
                List<Departure> departures = await departureDataFetcher.FetchDepartures(stop);
                stop.Departures = departures; 
                stops.Add(stop); 
            }

            return stops; 

        }

        /** 
         * Gets info about a stop from the Translink API 
         * stop: the stop number 
         * RETURNS: a StopInfo object with all info attached 
         * bayNo = -1 if there is no bay number 
         */
        public async Task<StopInfo> FetchStopInfo(int stopNo)
        {
            Stream stopXml = await FetchStopXml(stopNo);
            Debug.WriteLine("XML:" + stopXml.ToString());

            return DataParser.ParseStopInfo(stopXml); 
        }


        public async Task<List<StopInfo>> SearchStopInfo(double lat, double lon, int radius)
        {
            lat = Math.Round(lat, 6);
            lon = Math.Round(lon, 6); 

            Stream searchXml = await FetchSearchXml(lat, lon, radius);
            return DataParser.ParseStopsInfo(searchXml); 
        }

        public async Task<List<StopInfo>> SearchStopInfo(double lat, double lon, int radius, string route)
        {
            lat = Math.Round(lat, 6);
            lon = Math.Round(lon, 6);

            Stream searchXml = await FetchSearchXml(lat, lon, radius, route);
            return DataParser.ParseStopsInfo(searchXml); 
        }


        private async Task<Stream> FetchStopXml(int stopNo)
        {
            HttpResponseMessage response = await mHttpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops/" + stopNo +
                "?apikey=" + API_KEY);
            HttpContent content = response.Content;
            Stream contentStream = await content.ReadAsStreamAsync();
            return contentStream;
        }


        private async Task<Stream> FetchSearchXml(double lat, double lon, int radius) 
        {
            string uri = "http://api.translink.ca/rttiapi/v1/stops?apikey=" + API_KEY +
                "&lat=" + lat + "&long=" + lon + "&radius=" + radius;
            Debug.WriteLine("Getting data from " + uri); 
            HttpResponseMessage response = await mHttpClient.GetAsync(uri);
            HttpContent content = response.Content;
            
            Stream contentStream = await content.ReadAsStreamAsync();
            Debug.WriteLine("!!!!" + contentStream.ToString()); 
            return contentStream; 
        }

        private async Task<Stream> FetchSearchXml(double lat, double lon, int radius, string route)
        {
            HttpResponseMessage response = await mHttpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops?apikey=" + API_KEY +
                "&lat=" + lat + "&long=" + lon + "&radius=" + radius + "&routeNo=" + route);
            HttpContent content = response.Content;
            Stream contentStream = await content.ReadAsStreamAsync();
            return contentStream;
        }
    }
}
