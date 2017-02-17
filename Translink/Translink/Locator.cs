using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator; 

namespace Translink
{
    public static class Locator
    {
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetPositionAsync()
        {
            var geolocator = CrossGeolocator.Current;

            return await geolocator.GetPositionAsync();
        }
        

        
    }
}
