using FreshMvvm;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;
using Xamarin.Forms;
using Translink.Pages;
using RouteDirection = System.Tuple<string, string>;

namespace Translink.PageModels
{
    public class FavouriteRoutesPageModel : FreshBasePageModel
    {
        private IFavouritesDataService mDataService;

        public ObservableCollection<RouteDirection> RouteDirectionList { get; set; }

        public RouteDirection SelectedRouteDirection
        {
            get
            {
                return null; 
            }
            set
            {
                CoreMethods.PushPageModel<FavouriteRoutePageModel>(value);
                RaisePropertyChanged(); 
            }
        }

        public Command ClearFavouriteRoutes
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send(this, "DeleteFavouritesPrompt");
                });
            }
        }

        public FavouriteRoutesPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            RouteDirectionList = new ObservableCollection<RouteDirection>();
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e); 
            MessagingCenter.Subscribe<FavouriteRoutesPage>(
                this,
                "DeleteFavourites",
                async (page) => await DeleteFavourites());
            MessagingCenter.Subscribe<FavouriteRoutesPage, RouteDirection>(
                this, 
                "DeleteFavourite", 
                async (page, rd) => await DeleteFavourite(rd)); 
            await RefreshRouteList(); 
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            MessagingCenter.Unsubscribe<FavouriteRoutesPage>(this, "DeleteFavourites");
            MessagingCenter.Unsubscribe<FavouriteRoutesPage, RouteDirection>(this, "DeleteFavourite"); 
        }

        async Task RefreshRouteList()
        {
            List<RouteDirection> routeDirectionList = await mDataService.GetFavouriteRoutesAndDirections();
            RouteDirectionList.Clear();
            foreach (var r in routeDirectionList)
            {
                RouteDirectionList.Add(r); 
            }
        }

        async Task DeleteFavourites()
        {
            await mDataService.ClearFavouriteRoutes();
            RouteDirectionList.Clear();
        }

        async Task DeleteFavourite(RouteDirection rd)
        {
            await mDataService.RemoveFavouriteRoute(rd.Item1, rd.Item2);
            await RefreshRouteList(); 
        }
    }
}
