using System;
using System.Collections.Generic;

namespace Translink.Models
{
    public class Stop
    {
        private StopInfo mStopInfo;



        public String Name
        {
            get { return mStopInfo.Name; }
        }

        public List<Departure> Departures
        {
            get; set;
        }

        public List<string> Routes
        {
            get
            {
                return mStopInfo.Routes;
            }
        }

        public int Number
        {
            get { return mStopInfo.Number; }
        }

        // For departures
        public string StopDetail
        {
            get
            {
                string stopDetail = mStopInfo.Number + " ";
                if (Departures.Count >= 1)
                    stopDetail += Departures[0].Time;
                return stopDetail;
            }
        }

        public Stop(StopInfo stopInfo)
        {
            mStopInfo = stopInfo;
            Departures = new List<Departure>();
        }

        public Stop(StopInfo stopInfo, List<Departure> departures)
        {
            Departures = departures;
            mStopInfo = stopInfo;
        }

        public List<Departure> GetRouteDepartures(string route)
        {
            List<Departure> ans = new List<Departure>(); 
            foreach (Departure d in Departures)
            {
                if (Util.RouteEquals(route, d.RouteNumber))
                    ans.Add(d); 
            }
            return ans; 
        }
    }
}
