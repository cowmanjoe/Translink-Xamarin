using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    [ImplementPropertyChanged]
    public class Route
    {
        public string Number
        {
            get;
        }

        public int StopNumber
        {
            get { return mStop.Number; }
        }

        public string Direction { get; }

        public List<Departure> Departures
        {
            get
            {
                List<Departure> departures = new List<Departure>(); 
                foreach (Departure d in mStop.Departures)
                {
                    if (Departure.RouteEquals(Number, d.RouteNumber))
                        departures.Add(d); 
                }
                return departures; 
            }
        }

        private readonly Stop mStop;
        

        public string StopName
        {
            get
            {
                return mStop.Name; 
            }
        }

        public string NumberAndDirection
        {
            get { return Number + " " + Direction + "BOUND"; }
        }

        public Route(string number, string direction, Stop stop)
        {
            Number = number;
            Direction = direction; 
            mStop = stop;
        }

        public static Route GetRouteWithNumber(string number, IEnumerable<Route> routes)
        {
            foreach (Route route in routes)
            {
                if (route.Number == number)
                {
                    return route;
                }
            }

            return null;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Route r = (Route)obj;

            return Number == r.Number && Direction == r.Direction && StopNumber == r.StopNumber; 



        }

        public override int GetHashCode()
        {
            return Number.GetHashCode() * StopNumber.GetHashCode() * Direction.GetHashCode(); 
        }
    }
}
