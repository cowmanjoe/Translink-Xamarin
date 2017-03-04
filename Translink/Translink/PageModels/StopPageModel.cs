using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;
using Xamarin.Forms;

namespace Translink.PageModels
{
    public class StopPageModel : FreshMvvm.FreshBasePageModel
    {
        private readonly IStopDataService mStopDataService;
        private readonly IDepartureDataService mDepartureDataService;
        private readonly IFavouritesDataService mFavouritesDataService; 

        private StopInfo mStopInfo; 

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


        public Command FavouriteThisStop
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.AddFavouriteStop(mStopInfo); 
                });
            }
        }
        
        public StopPageModel(IDepartureDataService departureDataService, IStopDataService stopDataService, IFavouritesDataService favouritesDataService)
        {
            mDepartureDataService = departureDataService;
            mStopDataService = stopDataService;
            mFavouritesDataService = favouritesDataService; 
            AvailableRoutes = new List<string>();
            Departures = new ObservableCollection<Departure>(); 
        }

        // initData should be a StopInfo object
        public async override void Init(object initData)
        {
            base.Init(initData);
            mStopInfo = initData as StopInfo;

            StopName = mStopInfo.Name;
            StopNumber = mStopInfo.Number;

            

            AvailableRoutes.Add("All"); 
            foreach (string route in mStopInfo.Routes)
            {
                AvailableRoutes.Add(route); 
            }

            SelectedRouteIndex = 0;

            await RefreshDepartures(); 
        }
        

        private async Task RefreshDepartures()
        {
            mAllDepartures = await mDepartureDataService.SearchDepartures(StopNumber);

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
