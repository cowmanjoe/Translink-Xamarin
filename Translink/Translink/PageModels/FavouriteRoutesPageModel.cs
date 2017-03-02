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

        public ObservableCollection<RouteDirection> RouteList { get; set; }

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

        public FavouriteRoutesPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            RouteList = new ObservableCollection<RouteDirection>();
            MessagingCenter.Subscribe<FavouriteRoutesPage>(
                this,
                "On Appearing",
                async (page) => await RefreshRouteList());
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            

            await RefreshRouteList();
            
        }

        async Task RefreshRouteList()
        {
            List<string> routeList = await mDataService.GetFavouriteRoutesAndDirections();
            RouteList.Clear();
            foreach (string r in routeList)
            {
                RouteDirection routeDirection = new RouteDirection(r); 
                RouteList.Add(routeDirection);
            }
        }

        public Command ClearFavouriteRoutes
        {
            get
            {
                return new Command(async () =>
                {
                    await mDataService.ClearFavouriteRoutes();
                    RouteList.Clear();
                });
            }
        }

        [ImplementPropertyChanged]
        public class RouteDirection
        {
            public string RouteNumber { get; }
            public string Direction { get; }

            // Takes a string in the form <route number>:<direction>
            public RouteDirection(string routeDirection)
            {
                string[] routeAndDirection = routeDirection.Split(':');
                RouteNumber = routeAndDirection[0];
                Direction = routeAndDirection[1];
            }

            public RouteDirection(string routeNumber, string direction)
            {
                RouteNumber = routeNumber;
                Direction = direction;
            }
        }
    }
}
