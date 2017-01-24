using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel; 

using Xamarin.Forms;

namespace Translink
{
    public partial class MainPage : ContentPage
    {
        
        ObservableCollection<Departure> mDepartures;
        

        DepartureDataFetcher mDepartureDataFetcher;

        public MainPage()
        {
            InitializeComponent();
            mDepartures = new ObservableCollection<Departure>(); 
            DepartureListView.ItemsSource = mDepartures;
            mDepartureDataFetcher = new DepartureDataFetcher(); 

        }

        async void OnAddDeparturesRequested(object sender, EventArgs e)
        {
            int stopNumber = Convert.ToInt32(StopEntry.Text);
            
            if (RouteSwitch.IsToggled)
            {
                int routeNumber = Convert.ToInt32(RouteEntry.Text);
                List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(stopNumber, routeNumber);
                AddDepartures(departures); 
            }
            else
            {
                List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(stopNumber);
                AddDepartures(departures);
            }

        }

        

        void AddDepartures(List<Departure> departures)
        {
            foreach (Departure d in departures)
            {
                mDepartures.Add(d); 
            }
        }

        void OnClearDepartures(object sender, EventArgs e)
        {
            mDepartures.Clear(); 
        }

        void OnToggleRouteSwitch(object sender, EventArgs e)
        {
            if (RouteSwitch.IsToggled)
            {
                RouteEntry.IsVisible = true; 
            }
            else
            {
                RouteEntry.IsVisible = false; 
            }

        }
        
    }
}
