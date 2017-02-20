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

        Task<List<string>> GetFavouriteRouteNumbers(); 
    }
}
