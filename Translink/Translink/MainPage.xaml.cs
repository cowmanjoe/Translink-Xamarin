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


        public MainPage()
        {
            InitializeComponent();
            mDepartures = new ObservableCollection<Departure>(); 
            DepartureListView.ItemsSource = mDepartures; 
        }

        async void OnAddDeparturesRequested(object sender, EventArgs e)
        {
            int stopNumber = Convert.ToInt32(StopEntry.Text);

            List<Departure> departures = await DepartureDataFetcher.getDepartures(stopNumber);
            AddDepartures(departures); 
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
    }
}
