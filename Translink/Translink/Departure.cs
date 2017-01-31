using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    struct Departure
    {
        public Departure(string time, int stopNumber, string routeNumber)
        {
            Time = time;
            StopNumber = stopNumber;
            RouteNumber = routeNumber;
        }

        public string Time
        {
            get; 
        }

        public int StopNumber
        {
            get; 
        }

        public string RouteNumber
        {
            get; 
        }

        public string AsString
        {
            get { return StopNumber + " [" + RouteNumber + "] " + Time; }
        }
    }
}
