using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Translink.Exception; 
using Xamarin.Forms;
using Plugin.Connectivity;

namespace Translink
{
    public partial class MainPage : ContentPage
    {

        
        DepartureSearcher mDepartureSearcher;
        

        public MainPage()
        {
            InitializeComponent();

            mDepartureSearcher = new DepartureSearcher(); 
            DepartureListView.ItemsSource = mDepartureSearcher.Departures;
        }

        /** 
         * Requests data fetcher to grab departure data
         * sender: object that sent that fired the event 
         * EventArgs: arguments for the event 
         */
        async void OnAddDeparturesRequested(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("No Network Connection", "You cannot add departures without an internet connection", "OK");
                return; 
            }
            int stopNumber = Convert.ToInt32(StopEntry.Text);

            try
            {
                if (RouteSwitch.IsToggled)
                {
                    string routeNumber = RouteEntry.Text;

                    await mDepartureSearcher.SearchAndAddDepartures(stopNumber, routeNumber);
                }
                else
                {
                    await mDepartureSearcher.SearchAndAddDepartures(stopNumber);
                }
            }
            catch (InvalidStopException ex)
            {
                await DisplayAlert("Invalid Stop Number", ex.Message, "OK");
            }

            
        }


        

        /** 
         * Clears the departures from the ObservableList 
         * sender: object that sent that fired the event 
         * EventArgs: arguments for the event 
         */
        void OnClearDepartures(object sender, EventArgs e)
        {
            mDepartureSearcher.ClearDepartures();
            StopDataFetcher sdf = new StopDataFetcher();
            sdf.SearchStopInfo(49.268574, -123.172469, 1000); 
            
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
        

        async void OnRefreshDepartures(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("No Network Connection", "You cannot refresh departures without an internet connection", "OK");
                return;
            }
            await mDepartureSearcher.RefreshDepartures(); 
        }
    }
}
