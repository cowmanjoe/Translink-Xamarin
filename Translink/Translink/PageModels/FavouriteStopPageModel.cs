using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;

namespace Translink.PageModels
{
    public class FavouriteStopPageModel : FreshMvvm.FreshBasePageModel
    {
        private IDepartureDataService mStopDataService;

        public string StopName
        {
            get;
            private set; 
        }

        public int StopNumber
        {
            get;
            private set; 
        }

        public ObservableCollection<Departure> Departures
        {
            get;
            private set; 
        }

        public FavouriteStopPageModel(IDepartureDataService dataService)
        {
            mStopDataService = dataService; 
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            var stopInfo = initData as StopInfo;

            StopName = stopInfo.Name;
            StopNumber = stopInfo.Number;

            mStopDataService.ClearDepartures(); 
            await mStopDataService.SearchDepartures(stopInfo.Number);
            Departures = new ObservableCollection<Departure>();
            List<Departure> departures = mStopDataService.GetDepartures();
            
            foreach (Departure d in departures)
            {
                Departures.Add(d); 
            } 
        }



    }
}
