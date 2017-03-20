using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Models
{
    [ImplementPropertyChanged]
    public class StopInfo
    {
        public int Number { get; }
        public string Name { get; }
        public int BayNumber;
        public string OnStreet;
        public string AtStreet;
        public double Latitude;
        public double Longitude;
        public List<string> Routes;

        public string AvailableRoutesString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string r in Routes)
                {
                    sb.Append(r);
                    sb.Append(" ");
                }
                string availableRoutes = sb.ToString().Trim();
                return availableRoutes;
            }
        }

        public StopInfo(int number, string name)
        {
            Number = number;
            Name = name;
            Routes = new List<string>(); 
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            StopInfo s = (StopInfo)obj;

            
            if (Routes.Count != s.Routes.Count)
                return false;

            for (int i = 0; i < Routes.Count; i++)
            {
                if (!Util.RouteEquals(Routes[i], s.Routes[i]))
                    return false; 
            }

            return Number == s.Number &&
                Name == s.Name &&
                BayNumber == s.BayNumber;
        }
        public override int GetHashCode()
        {
            return 89 * Number.GetHashCode() * (Name ?? "").GetHashCode() * Latitude.GetHashCode() * Longitude.GetHashCode(); 
        }
    }

    
}
