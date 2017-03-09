﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink;
using Translink.Pages;
using Translink.Services;
using Xamarin.Forms;

namespace Translink.PageModels
{
    public class RoutePageModel : FreshMvvm.FreshBasePageModel
    {
        private IFavouritesDataService mFavouritesDataService; 

        public Route Route { get; set; }
        
        public bool IsBusy { get; private set; }

        public RoutePageModel (IFavouritesDataService dataService)
        {
            mFavouritesDataService = dataService;
            MessagingCenter.Subscribe<RoutePage>(this, "OnAppearing", async (sender) => await RefreshIsFavourite()); 
        }

        public async override void Init (object initData)
        {
            base.Init(initData);

            Route = initData as Route;

            await RefreshIsFavourite(); 
        }

        public bool IsFavourite
        {
            get; private set; 
        }

        public bool IsNotFavourite
        {
            get { return !IsFavourite; }
        }
        
        public Command FavouriteThisRoute
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.AddFavouriteRoute(Route.Number, Route.Direction);
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
                    await mFavouritesDataService.RemoveFavouriteRoute(Route.Number, Route.Direction);
                    IsFavourite = false;
                });
            }
        }


        private async Task RefreshIsFavourite()
        {
            List<string> favourites = await mFavouritesDataService.GetFavouriteRoutesAndDirections();
            IsFavourite = false;
            foreach (string r in favourites)
            {
                string[] routeAndDirection = r.Split(':');
                if (routeAndDirection[0] == Route.Number && routeAndDirection[1] == Route.Direction)
                {
                    IsFavourite = true;
                }
            }
        }
    }
}
