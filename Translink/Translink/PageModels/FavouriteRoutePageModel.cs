using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Pages;
using Translink.Services;
using Xamarin.Forms;
using Translink.Models;
using Translink.Exception;

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

        public bool Succeeded
        {
            get; private set; 
        }

        public bool Failed
        {
            get { return !Succeeded; }
        }

        public string FailedMessage
        {
            get; private set; 
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

            FailedMessage = "Unknown failure"; 
            Succeeded = true; 
            var routeDirection = initData as Tuple<string, string>;
            mRouteNumber = routeDirection.Item1;
            mDirection = routeDirection.Item2;
            try
            {
                Route = await mRouteDataService.GetNearestRoute(mRouteNumber, mDirection);
                if (Route == null)
                {
                    Succeeded = false;
                    FailedMessage = "No stops were found for the given route and direction";
                }
                else
                {
                    Succeeded = true;
                }
            }
            catch (LocationException e)
            {
                Succeeded = false; 
                if (e.GeolocationUnavailable)
                {
                    FailedMessage = "Location services are unavailable";
                }
                else if (e.GeolocationDisabled)
                {
                    FailedMessage = "Location services are disabled"; 
                }
                else
                {
                    FailedMessage = "Something wen wrong when finding your location"; 
                }
            }
            await RefreshIsFavourite();

            IsBusy = false; 
        }

        private async Task RefreshIsFavourite()
        {
            
            List<Tuple<string, string>> favourites = await mFavouritesDataService.GetFavouriteRoutesAndDirections();
            Tuple<string, string> thisRoute = new Tuple<string, string>(mRouteNumber, mDirection);

            IsFavourite = false;
            foreach (var r in favourites)
            {
                if (r.Equals(thisRoute))
                {
                    IsFavourite = true;
                }
            }
            
        }
    }
}
