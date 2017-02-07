using Android.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator; 

using Xamarin.Forms;

namespace Translink
{
    public partial class StopPage : ContentPage
    {
        private ObservableCollection<Stop> mStops; 

        const int SEARCH_RADIUS = 500; 

        public StopPage()
        {
            InitializeComponent();
            mStops = new ObservableCollection<Stop>();
            StopList.ItemsSource = mStops; 
        }


        async void OnRefreshNearestStops(object sender, EventArgs e)
        {
            StopDataFetcher stopDataFetcher = StopDataFetcher.Instance;
            List<Stop> stops = await StopLocator.FetchStopsAroundMe(SEARCH_RADIUS); 
            mStops.Clear(); 
            foreach (Stop stop in stops)
            {
                mStops.Add(stop); 
            }
        }
    }
}
