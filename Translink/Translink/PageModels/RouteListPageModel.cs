using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translink.Services;
using FreshMvvm;
using Xamarin.Forms;

namespace Translink.PageModels
{
    public class RouteListPageModel : FreshMvvm.FreshBasePageModel
    {
        private IRouteDataService mDataService;

        public List<Route> RouteList { get; set; }

        public Route SelectedRoute
        {
            get { return null; }
            set
            {
                CoreMethods.PushPageModel<RoutePageModel>(value);
                RaisePropertyChanged();
            }
        }


        public RouteListPageModel (IRouteDataService dataService) 
        {
            mDataService = dataService; 
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            
            RouteList = await mDataService.GetRoutes();
        }

        public Command RefreshRoutes
        {
            get
            {
                return new Command(async () =>
                {
                    RouteList = await mDataService.GetRoutes();
                }); 
            }
        }
    }
}
