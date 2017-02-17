using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Translink.Exception; 
using static Translink.StopDataFetcher;

namespace Translink
{
    public static class DataParser
    {
        /**
         * Parse the text that the API returns on a departure time request
         * data: the string received from Translink API
         * RETURNS: mapping of routes and directions in the form "route:direction" to a list of string representations of departure times
         **/
        public static Dictionary<string, List<string>> ParseDepartureTimes(Stream data)
        {
            Dictionary<string, List<string>> routeDictionary = new Dictionary<string, List<string>>();

            XDocument xDoc = XDocument.Load(data);

            CheckDeparturesForErrors(xDoc);


            var nextBuses = xDoc.Descendants("NextBus");

            foreach(var nextBus in nextBuses)
            {
                string routeNo = nextBus.Element("RouteNo").Value;
                string direction = nextBus.Element("Direction").Value;

                var schedules = nextBus.Descendants("Schedule");
                List<string> times = new List<string>();
                foreach(var schedule in schedules)
                {
                    String time = schedule.Element("ExpectedLeaveTime").Value;
                    time = time.Substring(0, time.IndexOf('m') + 1);
                    times.Add(time);
                }
                routeDictionary.Add(routeNo + ":" + direction, times);
            }

            Debug.WriteLine("Parsing this: " + data);


            foreach (string r in routeDictionary.Keys)
            {
                Debug.WriteLine("Route #" + r + " being returned");
            }

            return routeDictionary;
        }

        private static void CheckDeparturesForErrors(XDocument xml)
        {

            if (xml.Root.Name == "Error")
            {
                string errorCode = xml.Root.Element("Code").Value;
                if (errorCode == "3005")
                {
                    return;
                }
                else if (errorCode == "3001")
                {
                    throw new InvalidStopException("Stop must be a valid 5 digit number.");
                }
                else if (errorCode == "3002")
                {
                    throw new InvalidStopException("Stop not found.");
                }
                else
                {
                    throw new TranslinkAPIErrorException(Convert.ToInt32(errorCode));
                }
            }
        }

        public static StopInfo ParseStopInfo(Stream stream)
        {
            XDocument xDoc = XDocument.Load(stream);
            return ParseStopInfo(xDoc.Root);
        }



        public static List<StopInfo> ParseStopsInfo(Stream stream)
        {
            List<StopInfo> stops = new List<StopInfo>();

            XDocument xDoc = XDocument.Load(stream);

            var stopContainer = xDoc.Descendants("Stop");

            foreach (var stopElem in stopContainer)
            {
                StopInfo stopInfo = ParseStopInfo(stopElem);
                stops.Add(stopInfo);
            }

            return stops;
        }


        private static StopInfo ParseStopInfo(XElement stopElement)
        {
            StopInfo stopInfo = new StopInfo();

            int stopNo = Convert.ToInt32(stopElement.Element("StopNo").Value);
            stopInfo.stopNo = stopNo;

            string name = stopElement.Element("Name").Value;
            stopInfo.name = name.Trim();

            string bayNo = stopElement.Element("BayNo").Value;
            if (bayNo != "N")
                stopInfo.bayNo = Convert.ToInt32(bayNo);
            else
                stopInfo.bayNo = -1;

            string onStreet = stopElement.Element("OnStreet").Value;
            stopInfo.onStreet = onStreet.Trim();

            string atStreet = stopElement.Element("AtStreet").Value;
            stopInfo.atStreet = atStreet.Trim();

            double latitude = Convert.ToDouble(stopElement.Element("Latitude").Value);
            stopInfo.latitude = latitude;

            double longitude = Convert.ToDouble(stopElement.Element("Longitude").Value);
            stopInfo.longitude = longitude;

            List<string> routes = new List<string>();
            string[] r = stopElement.Element("Routes").Value.Split(',');

            for (int i = 0; i < r.Length; i++)
            {
                routes.Add(r[i].Trim());
            }
            stopInfo.routes = routes;


            Debug.WriteLine("Stop info received: ");
            Debug.WriteLine("  stopNo = " + stopInfo.stopNo +
                "\n  name = " + stopInfo.name +
                "\n  onStreet = " + stopInfo.onStreet +
                "\n  atStreet = " + stopInfo.atStreet +
                "\n  latitude = " + stopInfo.latitude +
                "\n  longitude = " + stopInfo.longitude);
            Debug.WriteLine("  Routes: ");
            foreach (string route in stopInfo.routes)
            {
                Debug.WriteLine("    " + route);
            }

            Debug.WriteLine(stopInfo.bayNo);


            return stopInfo;
        }
    }
}
