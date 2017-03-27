using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Translink.Exception;
using Translink.Models;
using RouteDirection = System.Tuple<string, string>;

namespace Translink
{
    public static class DataParser
    {
        /**
         * Parse the text that the API returns on a departure time request
         * data: the stream received from Translink API
         * RETURNS: mapping of route/direction doubles to a list of DateTime departure times
         **/
        public static Dictionary<RouteDirection, List<DateTime>> ParseDepartureTimes(Stream data)
        {
            Dictionary<RouteDirection, List<DateTime>> routeDictionary = new Dictionary<RouteDirection, List<DateTime>>();

            XDocument xDoc = XDocument.Load(data);

            CheckDeparturesForErrors(xDoc);

            DateTime now = DateTime.Now; 

            var nextBuses = xDoc.Descendants("NextBus");

            foreach(var nextBus in nextBuses)
            {
                string routeNo = nextBus.Element("RouteNo").Value;
                string direction = nextBus.Element("Direction").Value;

                var schedules = nextBus.Descendants("Schedule");
                List<DateTime> times = new List<DateTime>();

                foreach (var schedule in schedules)
                {
                    int expectedCountdown = Convert.ToInt32(schedule.Element("ExpectedCountdown").Value);

                    DateTime leaveTime = now.AddMinutes(expectedCountdown);

                    times.Add(leaveTime); 
                }
                
                routeDictionary.Add(new RouteDirection(routeNo, direction), times);
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
            CheckStopInfoForErrors(xDoc); 
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
            

            int stopNo = Convert.ToInt32(stopElement.Element("StopNo").Value);
            string name = stopElement.Element("Name").Value;
            name = name.Trim(); 
            StopInfo stopInfo = new StopInfo(stopNo, name); 

            string bayNo = stopElement.Element("BayNo").Value;
            if (bayNo != "N")
                stopInfo.BayNumber = Convert.ToInt32(bayNo);
            else
                stopInfo.BayNumber = -1;

            string onStreet = stopElement.Element("OnStreet").Value;
            stopInfo.OnStreet = onStreet.Trim();

            string atStreet = stopElement.Element("AtStreet").Value;
            stopInfo.AtStreet = atStreet.Trim();

            double latitude = Convert.ToDouble(stopElement.Element("Latitude").Value);
            stopInfo.Latitude = latitude;

            double longitude = Convert.ToDouble(stopElement.Element("Longitude").Value);
            stopInfo.Longitude = longitude;

            List<string> routes = new List<string>();
            string[] r = stopElement.Element("Routes").Value.Split(',');

            for (int i = 0; i < r.Length; i++)
            {
                routes.Add(r[i].Trim());
            }
            stopInfo.Routes = routes;

            return stopInfo;
        }

        private static void CheckStopInfoForErrors(XDocument xml)
        {

            if (xml.Root.Name == "Error")
            {
                string errorCode = xml.Root.Element("Code").Value;
                if (errorCode == "1001")
                {
                    throw new InvalidStopException("Stop must be a valid 5 digit number.");
                }
                else if (errorCode == "1002")
                {
                    throw new InvalidStopException("Stop not found.");
                }
                else
                {
                    throw new TranslinkAPIErrorException(Convert.ToInt32(errorCode));
                }
            }
        }
    }
}
