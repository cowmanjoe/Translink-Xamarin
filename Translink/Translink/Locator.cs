using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using System.Diagnostics;
using Plugin.Geolocator.Abstractions;
using Translink.Exception;

namespace Translink
{
    public static class Locator
    {
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetPositionAsync()
        {
            IGeolocator geolocator = CrossGeolocator.Current;

            if (!geolocator.IsGeolocationEnabled)
            {
                throw new LocationException(true, false); 
            }

            if (!geolocator.IsGeolocationAvailable)
            {
                throw new LocationException(false, true); 
            }

            try
            {
                return await geolocator.GetPositionAsync();
            }
            catch (Plugin.Geolocator.Abstractions.GeolocationException e)
            {
                throw new LocationException(e.Message, false, false); 
            }
        }
        

        
    }
}
