using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public interface IFavouritesDataService
    {
        Task<List<StopInfo>> GetFavouriteStopInfos();

        // Should return strings in the form <route number>:<direction>
        Task<List<string>> GetFavouriteRoutesAndDirections();

        Task AddFavouriteStop(StopInfo stopInfo);

        Task AddFavouriteRoute(string routeNumber, string direction);

        Task RemoveFavouriteStop(int stopNumber);

        Task RemoveFavouriteRoute(string routeNumber, string direction);

        Task ClearFavouriteStops();

        Task ClearFavouriteRoutes(); 
    }
}
