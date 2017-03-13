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

        public FavouriteRoutesPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            RouteDirectionList = new ObservableCollection<Tuple<string, string>>();
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
            List<Tuple<string, string>> routeDirectionList = await mDataService.GetFavouriteRoutesAndDirections();
            RouteDirectionList.Clear();
            foreach (var r in routeDirectionList)
            {
                RouteDirectionList.Add(r); 
            }
        }

        public Command ClearFavouriteRoutes
        {
            get
            {
                return new Command(async () =>
                {
                    await mDataService.ClearFavouriteRoutes();
                    RouteDirectionList.Clear();
                });
            }
        }
    }
}
