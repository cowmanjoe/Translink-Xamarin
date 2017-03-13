using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Models
{
    [ImplementPropertyChanged]
    public class Departure : IComparable<Departure>
    {
        //private Stop mStop;

        public DateTime Time
        {
            get;
        }

        public string TimeString
        {
            get
            {
                return string.Format("{0:t}", Time); 
            }
        }

        public int StopNumber
        {
            get;
        }

        public string RouteNumber
        {
            get;
        }

        public string Direction { get; }

        public string AsString
        {
            get { return StopNumber + " [" + RouteNumber + "] " + Time; }
        }

        public Departure(DateTime time, int stopNo, string routeNumber, string direction)
        {
            Time = time;
            StopNumber = stopNo;
            RouteNumber = routeNumber;
            Direction = direction; 
        }

        public Departure(DateTime time, Stop stop, string routeNumber, string direction)
        {
            Time = time;
            StopNumber = stop.Number;
            RouteNumber = routeNumber;
            Direction = direction;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Departure d = (Departure)obj;

            return Time == d.Time && StopNumber == d.StopNumber && Util.RouteEquals(RouteNumber, d.RouteNumber) && Direction == d.Direction;
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode() * RouteNumber.GetHashCode() * base.GetHashCode();
        }

        public int CompareTo(Departure other)
        {
            return Time.CompareTo(other.Time); 
        }
    }
}
