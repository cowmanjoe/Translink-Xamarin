using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Models;

namespace Translink.Services
{
    public interface IRouteDataService
    {
        Task<List<Route>> GetRoutes();

        Task<Route> GetNearestRoute(string number, string direction); 
    }
}
