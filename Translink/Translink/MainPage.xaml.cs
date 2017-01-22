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

        void OnAddDepartures(object sender, EventArgs e)
        {
            int routeNumber = Convert.ToInt32(RouteEntry.Text);

            mDepartures.Add(new Departure("5:55", 50585, 3)); 
        }

        void OnClearDepartures(object sender, EventArgs e)
        {
            mDepartures.Clear(); 
        }
    }
}
