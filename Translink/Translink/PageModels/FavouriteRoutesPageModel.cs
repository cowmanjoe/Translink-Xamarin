using FreshMvvm;
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
    public class FavouriteRoutesPageModel : FreshBasePageModel
    {
        private IFavouritesDataService mDataService;

        public ObservableCollection<string> RouteList { get; set; }

        public FavouriteRoutesPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            RouteList = new ObservableCollection<string>();
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            await RefreshRouteList();
        }

        async Task RefreshRouteList()
        {
            List<string> routeList = await mDataService.GetFavouriteRouteNumbers();
            RouteList.Clear();
            foreach (string r in routeList)
            {
                RouteList.Add(r);
            }
        }

        public Command ClearFavouriteRoutes
        {
            get
            {
                return new Command(async () => {
                    await mDataService.ClearFavouriteRoutes();
                    RouteList.Clear();
                });
            }
        }

    }
}
