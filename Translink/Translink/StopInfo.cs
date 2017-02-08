using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public struct StopInfo
    {
        public int stopNo;
        public string name;
        public int bayNo;
        public string onStreet;
        public string atStreet;
        public double latitude;
        public double longitude;
        public List<string> routes;

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            StopInfo s = (StopInfo)obj;

            if (routes.Count != s.routes.Count)
                return false;

            for (int i = 0; i < routes.Count; i++)
            {
                Departure.RouteEquals(routes[i], s.routes[i]);
            }


            return stopNo == s.stopNo &&
                name == s.name &&
                bayNo == s.bayNo &&
                onStreet == s.onStreet &&
                atStreet == s.atStreet &&
                latitude.Equals(s.latitude) &&
                longitude.Equals(s.longitude);
        }
        public override int GetHashCode()
        {
            return 89 * stopNo.GetHashCode() * name.GetHashCode() * latitude.GetHashCode() * longitude.GetHashCode(); 
        }
    }

    
}
