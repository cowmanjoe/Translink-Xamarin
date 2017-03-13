using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Exception
{
    public class LocationException : System.Exception
    {
        public bool GeolocationDisabled;
        public bool GeolocationUnavailable; 

        public LocationException(bool disabled, bool unavailable)
        {
            GeolocationDisabled = disabled;
            GeolocationUnavailable = unavailable; 
        }

        public LocationException(string message, bool disabled, bool unavailable) 
            : base(message)
        {
            GeolocationDisabled = disabled;
            GeolocationUnavailable = unavailable;
        }
    }
}
