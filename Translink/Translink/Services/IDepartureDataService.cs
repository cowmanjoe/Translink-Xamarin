using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Models;

namespace Translink.Services
{
    public interface IDepartureDataService
    {
        Task<List<Departure>> SearchDepartures(int stop);

        Task<List<Departure>> SearchDepartures(int stop, string route);
    }
}
