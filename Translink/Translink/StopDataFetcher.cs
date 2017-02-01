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


        public struct StopInfo
        {
            public int stopNo;
            public string name;
            public int bayNo;
            public string onStreet;
            public string atStreet;
            public double latitude;
            public double longitude;
            public List<string> routes;
            
        }


        public StopDataFetcher()
        {
            mHttpClient = new HttpClient(); 
        }

        /** 
         * Gets info about a stop from the Translink API 
         * stop: the stop number 
         * RETURNS: a StopInfo object with all info attached 
         * bayNo = -1 if there is no bay number 
         */
        public async Task<StopInfo> FetchStopInfo(int stop)
        {
            Stream stopXml = await FetchStopXml(stop);
            Debug.WriteLine("XML:" + stopXml.ToString());

            return DataParser.ParseStopInfo(stopXml); 
        }


        public async Task<List<StopInfo>> SearchStopInfo(double lat, double lon, int radius)
        {
            Stream searchXml = await FetchSearchXml(lat, lon, radius);
            Debug.WriteLine("XML:" + searchXml.ToString());

            return DataParser.ParseStopsInfo(searchXml); 
        }

        public async Task<List<StopInfo>> SearchStopInfo(double lat, double lon, int radius, string route)
        {
            throw new NotImplementedException(); 
        }


        private async Task<Stream> FetchStopXml(int stop)
        {
            HttpResponseMessage response = await mHttpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops/" + stop +
                "?apikey=" + API_KEY);
            HttpContent content = response.Content;
            Stream contentStream = await content.ReadAsStreamAsync();
            return contentStream;
        }


        private async Task<Stream> FetchSearchXml(double lat, double lon, int radius) 
        {
            HttpResponseMessage response = await mHttpClient.GetAsync(
                "http://api.translink.ca/rttiapi/v1/stops?apikey=" + API_KEY +
                "&lat=" + lat + "&long=" + lon + "&radius=" + radius);
            HttpContent content = response.Content;
            Stream contentStream = await content.ReadAsStreamAsync();
            return contentStream; 
        }
    }
}
