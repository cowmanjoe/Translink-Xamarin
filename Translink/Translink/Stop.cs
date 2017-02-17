using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public class Stop
    {
        private StopInfo mStopInfo;



        public String Name
        {
            get { return mStopInfo.name; }
        }

        public List<Departure> Departures
        {
            get; set;
        }

        public List<string> Routes
        {
            get
            {
                return mStopInfo.routes;
            }
        }

        public int Number
        {
            get { return mStopInfo.stopNo; }
        }

        // For departures
        public string StopDetail
        {
            get
            {
                string stopDetail = mStopInfo.stopNo + " ";
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
    }
}
