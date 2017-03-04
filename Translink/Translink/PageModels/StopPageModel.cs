using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;

namespace Translink.PageModels
{
    public class StopPageModel : FreshMvvm.FreshBasePageModel
    {
        private readonly IStopDataService mStopDataService;
        private readonly IDepartureDataService mDepartureDataService; 

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

        public List<string> AvailableRoutes
        {
            get;
            private set; 
        }

        private List<Departure> mAllDepartures; 
        public ObservableCollection<Departure> Departures
        {
            get;
            private set; 
        }

        private int mSelectedRouteIndex; 
        public int SelectedRouteIndex
        {
            get
            {
                return mSelectedRouteIndex; 
            }
            set
            {
                mSelectedRouteIndex = value;
                FilterDepartures(); 
            } 
        }      
        
        public StopPageModel(IDepartureDataService departureDataService, IStopDataService stopDataService)
        {
            mDepartureDataService = departureDataService;
            mStopDataService = stopDataService;
            AvailableRoutes = new List<string>();
            Departures = new ObservableCollection<Departure>(); 
        }

        // initData should be a StopInfo object
        public async override void Init(object initData)
        {
            base.Init(initData);
            var stopInfo = initData as StopInfo;

            StopName = stopInfo.Name;
            StopNumber = stopInfo.Number;

            

            AvailableRoutes.Add("All"); 
            foreach (string route in stopInfo.Routes)
            {
                AvailableRoutes.Add(route); 
            }

            SelectedRouteIndex = 0;

            await RefreshDepartures(); 
        }
        

        private async Task RefreshDepartures()
        {
            mDepartureDataService.ClearDepartures();
            await mDepartureDataService.SearchDepartures(StopNumber);
            mAllDepartures = mDepartureDataService.GetDepartures();

            FilterDepartures(); 
        }


        private void FilterDepartures()
        {
            Departures.Clear(); 

            foreach (Departure d in mAllDepartures)
            {
                if (AvailableRoutes[mSelectedRouteIndex] == "All" ||
                    d.RouteNumber == AvailableRoutes[mSelectedRouteIndex])
                {
                    Departures.Add(d); 
                }
            }
        }
    }
}
