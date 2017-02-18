using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translink.Services;
using FreshMvvm;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Translink.PageModels
{
    public class RouteListPageModel : FreshMvvm.FreshBasePageModel
    {
        private IRouteDataService mDataService;

        public ObservableCollection<Route> RouteList { get; set; }

        public Route SelectedRoute
        {
            get { return null; }
            set
            {
                CoreMethods.PushPageModel<RoutePageModel>(value);
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get;
            private set; 
        }


        public RouteListPageModel (IRouteDataService dataService) 
        {
            mDataService = dataService; 
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            RouteList = new ObservableCollection<Route>();
            IsBusy = false; 
        }

        public Command RefreshRoutes
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true; 
                    List<Route> routeList = await mDataService.GetRoutes();
                    RouteList.Clear(); 
                    foreach (Route r in routeList)
                    {
                        RouteList.Add(r); 
                    }
                    IsBusy = false; 
                }); 
            }
        }
    }
}
