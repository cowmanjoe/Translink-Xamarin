using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Models;
using Xamarin.Forms;

namespace Translink
{
    public partial class RoutePage : ContentPage
    {
        private ObservableCollection<Route> mRoutes;

        const int SEARCH_RADIUS = 500;

        public RoutePage()
        {
            InitializeComponent();
            
            mRoutes = new ObservableCollection<Route>();
            RouteList.ItemsSource = mRoutes;
        }


        async void OnRefreshNearestStops(object sender, EventArgs e)
        {

            List<Route> routeList = await RouteLocator.FetchRoutesWithStopsAroundMe(SEARCH_RADIUS);
            mRoutes.Clear();
            foreach (Route r in routeList)
            {
                mRoutes.Add(r);
            }


        }

    }
}
