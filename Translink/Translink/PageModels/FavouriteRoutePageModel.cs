using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;

namespace Translink.PageModels
{
    public class FavouriteRoutePageModel : FreshMvvm.FreshBasePageModel
    {
        private IRouteDataService mRouteDataService;

        public Route Route
        {
            get;
            private set;
        }

        public string NumberAndDirection
        {
            get;
            private set; 
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

        public FavouriteRoutePageModel (IRouteDataService dataService)
        {
            mRouteDataService = dataService; 
        }

        public async override void Init (object initData)
        {
            base.Init(initData);
            StopsAvailable = true; 
            var routeDirection = initData as FavouriteRoutesPageModel.RouteDirection;

            NumberAndDirection = routeDirection.RouteNumber + " " + routeDirection.Direction; 

            Route = await mRouteDataService.GetNearestRoute(routeDirection.RouteNumber, routeDirection.Direction);
            if (Route == null)
                StopsAvailable = false;
            else
                StopsAvailable = true; 
        }


    }
}
