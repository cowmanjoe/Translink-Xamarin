using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink;
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
        }

        public override void Init (object initData)
        {
            base.Init(initData);

            Route = initData as Route;
        }

        public Command AddToFavourites
        {
            get
            {
                return new Command(async () =>
                {
                    await mFavouritesDataService.AddFavouriteRoute(Route.Number);
                });
            }
        }
    }
}
