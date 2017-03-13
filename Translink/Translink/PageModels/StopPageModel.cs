using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Pages;
using Translink.Services;
using Xamarin.Forms;
using Translink.Models;

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

        public bool IsFavourite
        {
            get; private set;
        }

        public bool IsNotFavourite
        {
            get { return !IsFavourite; }
        }

        public Command FavouriteThisStop
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.AddFavouriteStop(mStopInfo);
                    IsFavourite = true; 
                });
            }
        }

        public Command UnfavouriteThisStop
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.RemoveFavouriteStop(mStopInfo.Number);
                    IsFavourite = false;
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
            MessagingCenter.Subscribe<StopPage>(this, "OnAppearing", async (sender) => await RefreshIsFavourite());
        }

        // initData should be a StopInfo object
        public async override void Init(object initData)
        {
            base.Init(initData);
            mStopInfo = initData as StopInfo;

            StopName = mStopInfo.Name;
            StopNumber = mStopInfo.Number;



            AvailableRoutes.Add("All");

            await RefreshIsFavourite(); 

            await RefreshDepartures(); 
        }
        

        private async Task RefreshDepartures()
        {
            mAllDepartures = await mDepartureDataService.SearchDepartures(StopNumber);

            mAllDepartures.Sort(); 

            foreach (Departure d in mAllDepartures)
            {
                if (!AvailableRoutes.Contains(d.RouteNumber))
                {
                    AvailableRoutes.Add(d.RouteNumber);
                }
            }

            MessagingCenter.Send(this, "RefreshRoutes"); 

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

        private async Task RefreshIsFavourite()
        {
            List<StopInfo> favourites = await mFavouritesDataService.GetFavouriteStopInfos();
            IsFavourite = false;
            foreach (StopInfo s in favourites)
            {
                if (mStopInfo.Number == s.Number)
                {
                    IsFavourite = true;
                }
            }
        }
        
    }
}
