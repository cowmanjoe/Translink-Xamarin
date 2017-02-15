using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Services
{
    public interface IDepartureDataService
    {
        List<Departure> GetDepartures();

        Task SearchDepartures(int stop); 

        Task SearchDepartures(int stop, string route);

        Task RefreshDepartures();

        void ClearDepartures(); 
    }
}
