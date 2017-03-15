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

namespace Translink.PageModels
{
    public class FavouriteRoutesPageModel : FreshBasePageModel
    {
        private IFavouritesDataService mDataService;

        public ObservableCollection<Tuple<string, string>> RouteDirectionList { get; set; }

        public Tuple<string, string> SelectedRouteDirection
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
            RouteDirectionList = new ObservableCollection<Tuple<string, string>>();
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e); 
            MessagingCenter.Subscribe<FavouriteRoutesPage>(
                this,
                "DeleteFavourites",
                async (page) => await DeleteFavourites());
            await RefreshRouteList(); 
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            MessagingCenter.Unsubscribe<FavouriteRoutesPage>(this, "DeleteFavourites"); 
        }

        async Task RefreshRouteList()
        {
            List<Tuple<string, string>> routeDirectionList = await mDataService.GetFavouriteRoutesAndDirections();
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
    }
}
