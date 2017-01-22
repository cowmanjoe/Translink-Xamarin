using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    class Departure
    {
        private string mTime;
        private int mStopNumber;
        private int mRouteNumber;

        public Departure(string time, int stopNumber, int routeNumber)
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

        public int RouteNumber
        {
            get { return mRouteNumber; }
        }

        public string AsString
        {
            get { return mStopNumber + " [" + mRouteNumber + "] " + mTime; }
        }
    }
}
