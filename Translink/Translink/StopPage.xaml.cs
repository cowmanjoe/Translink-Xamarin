using Android.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Translink
{
    public partial class StopPage : ContentPage
    {
        private StopDataFetcher mStopDataFetcher;

        private ObservableCollection<Stop> mStops; 

        const int SEARCH_RADIUS = 1000; 

        public StopPage()
        {
            InitializeComponent();
            mStops = new ObservableCollection<Stop>();

            mStopDataFetcher = new StopDataFetcher();
            

            StopInfo stop1 = new StopInfo();
            stop1.stopNo = 51516;
            stop1.name = "EB W KING EDWARD AVE FS MANITOBA ST";
            stop1.bayNo = -1;
            stop1.onStreet = "W KING EDWARD AVE";
            stop1.atStreet = "MANITOBA ST";
            stop1.latitude = 49.248820;
            stop1.longitude = -123.107050;
            stop1.routes = new List<string>();
            stop1.routes.Add("025");

            List<Departure> departures = new List<Departure>();
            departures.Add(new Translink.Departure("5:55", 51516, "025")); 

            Stop stop = new Stop(stop1, departures);
            mStops.Add(stop); 
            StopList.ItemsSource = mStops; 
        }


        public async Task UpdateNearestStops(Location location)
        {
            List<Stop> stops = await mStopDataFetcher.SearchStopsWithDepartures(location.Latitude, location.Longitude, SEARCH_RADIUS);
            mStops.Clear(); 
            foreach (Stop stop in stops)
            {
                mStops.Add(stop); 
            }
        }
    }
}
