using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Pages;
using Translink.Services;
using Xamarin.Forms;

namespace Translink.PageModels
{
    public class FavouriteRoutePageModel : FreshMvvm.FreshBasePageModel
    {
        private readonly IRouteDataService mRouteDataService;
        private readonly IFavouritesDataService mFavouritesDataService;

        private string mRouteNumber;
        private string mDirection; 

        public Route Route
        {
            get;
            private set;
        }

        public string NumberAndDirection
        {
            get { return mRouteNumber + " " + mDirection; }
        }

        public bool StopsAvailable
        {
            get;
            private set; 
        }

        public bool StopsNotAvailable
        {
            get { return !StopsAvailable; }
        }

        public bool IsFavourite
        {
            get; private set; 
        }

        public bool IsNotFavourite
        {
            get { return !IsFavourite; }
        }

        public bool IsBusy
        {
            get; private set; 
        }

        public Command FavouriteThisRoute
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.AddFavouriteRoute(mRouteNumber, mDirection);
                    IsFavourite = true; 
                });
            }
        }

        public Command UnfavouriteThisRoute
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.RemoveFavouriteRoute(mRouteNumber, mDirection);
                    IsFavourite = false; 
                });

            }
        }

        public FavouriteRoutePageModel (IRouteDataService dataService, IFavouritesDataService favouritesDataService)
        {
            mRouteDataService = dataService;
            mFavouritesDataService = favouritesDataService;
            MessagingCenter.Subscribe<FavouriteRoutePage>(this, "OnAppearing", async (sender) => await RefreshIsFavourite()); 
        }

        public async override void Init (object initData)
        {
            base.Init(initData);
            IsBusy = true; 

            StopsAvailable = true; 
            var routeDirection = initData as FavouriteRoutesPageModel.RouteDirection;
            mRouteNumber = routeDirection.RouteNumber;
            mDirection = routeDirection.Direction; 

            Route = await mRouteDataService.GetNearestRoute(mRouteNumber, mDirection);
            if (Route == null)
                StopsAvailable = false;
            else
                StopsAvailable = true;

            await RefreshIsFavourite();

            IsBusy = false; 
        }

        private async Task RefreshIsFavourite()
        {
            List<string> favourites = await mFavouritesDataService.GetFavouriteRoutesAndDirections();
            IsFavourite = false;
            foreach (string r in favourites)
            {
                string[] routeAndDirection = r.Split(':');
                if (routeAndDirection[0] == mRouteNumber && routeAndDirection[1] == mDirection)
                {
                    IsFavourite = true;
                }
            }
        }
    }
}
