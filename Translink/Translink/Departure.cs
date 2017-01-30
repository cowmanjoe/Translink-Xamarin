using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    class Departure
    {
        private readonly string mTime;
        private readonly int mStopNumber;
        private readonly string mRouteNumber;

        public Departure(string time, int stopNumber, string routeNumber)
        {
            mTime = time;
            mStopNumber = stopNumber;
            mRouteNumber = routeNumber;
        }


        public string Time
        {
            get { return mTime; }
        }

        public int StopNumber
        {
            get { return mStopNumber; }
        }

        public string RouteNumber
        {
            get { return mRouteNumber; }
        }

        public string AsString
        {
            get { return mStopNumber + " [" + mRouteNumber + "] " + mTime; }
        }
    }
}
