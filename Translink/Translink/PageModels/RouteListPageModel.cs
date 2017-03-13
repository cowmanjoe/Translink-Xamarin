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
using Translink.Models;
using Translink.Exception;

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
                if (value != null)
                {
                    CoreMethods.PushPageModel<RoutePageModel>(value);
                    RaisePropertyChanged();
                }
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

        public override void Init(object initData)
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
                    try
                    {
                        List<Route> routeList = await mDataService.GetRoutes();
                        List<Route> sortedRouteList = routeList.OrderBy(o => o.Number).ToList();
                        RouteList.Clear();
                        foreach (Route r in sortedRouteList)
                        {
                            RouteList.Add(r);
                        }
                    }
                    catch (LocationException e)
                    {
                        Alert alert;
                        if (e.GeolocationUnavailable)
                        {
                            alert = new Alert("Error", "Location services are unavailable.", "OK");
                        }
                        else if (e.GeolocationDisabled)
                        {
                            alert = new Alert("Error", "Location services are disabled.", "OK");
                        }
                        else
                        {
                            alert = new Alert("Error", "Something went wrong when finding your location.", "OK");
                        }
                        MessagingCenter.Send(this, "Display Alert", alert);
                    }
                    IsBusy = false; 
                }); 
            }
        }

        
    }
}
