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

namespace Translink
{
    public static class DataParser
    {
        /**
         * Parse the text that the API returns on a departure time request
         * data: the string received from Translink API
         * RETURNS: mapping of route/direction doubles to a list of DateTime departure times
         **/
        public static Dictionary<Tuple<string, string>, List<DateTime>> ParseDepartureTimes(Stream data)
        {
            Dictionary<Tuple<string, string>, List<DateTime>> routeDictionary = new Dictionary<Tuple<string, string>, List<DateTime>>();

            XDocument xDoc = XDocument.Load(data);

            CheckDeparturesForErrors(xDoc);


            var nextBuses = xDoc.Descendants("NextBus");

            foreach(var nextBus in nextBuses)
            {
                string routeNo = nextBus.Element("RouteNo").Value;
                string direction = nextBus.Element("Direction").Value;

                var schedules = nextBus.Descendants("Schedule");
                List<DateTime> times = new List<DateTime>();
                foreach (var schedule in schedules)
                {
                    string dateTimeString = schedule.Element("ExpectedLeaveTime").Value;
                    string[] timeAndDateString = dateTimeString.Split(' ');
                    string timeString = timeAndDateString[0];
                    string dateString = timeAndDateString[1]; 

                    string hourString = timeString.Substring(0, timeString.IndexOf(':'));
                    int hour = Convert.ToInt32(hourString);

                    char[] aAndP = { 'a', 'p' };
                    string minuteString = timeString.Substring(timeString.IndexOf(':') + 1, timeString.IndexOfAny(aAndP) - timeString.IndexOf(':') - 1);

                    string amOrPmString = timeString.Substring(timeString.IndexOfAny(aAndP), 2);


                    if (amOrPmString.Equals("pm"))
                        hour += 12;

                    if (hour == 24)
                        hour = 0; 

                    string[] dateParts = dateString.Split('-');
                    string yearString = dateParts[0];
                    string monthString = dateParts[1];
                    string dayString = dateParts[2];


                    DateTime dateTime = new DateTime(
                        Convert.ToInt32(yearString),
                        Convert.ToInt32(monthString),
                        Convert.ToInt32(dayString),
                        hour,
                        Convert.ToInt32(minuteString), 
                        0); 
                    
                    times.Add(dateTime);
                }
                routeDictionary.Add(new Tuple<string, string>(routeNo, direction), times);
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
