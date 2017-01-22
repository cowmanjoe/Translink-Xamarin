using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Translink
{
    class OldMainPage : ContentPage
    {
        Entry mRouteEntry;
        Button mAddButton;
        ListView mDepartureListView;
        List<Departure> mDepartures;

        public OldMainPage()
        {
            

            mRouteEntry = new Entry
            {
                HorizontalTextAlignment = TextAlignment.Start,
                Keyboard = Keyboard.Numeric
            };

            mAddButton = new Button
            {
                HorizontalOptions = LayoutOptions.Start,
                Text = "Add",
                Command = new Command(() => addDepartures())
            };

            mDepartures = new List<Departure>();

            mDepartureListView = new ListView
            {
                HorizontalOptions = LayoutOptions.Fill,
                ItemsSource = mDepartures

            };

            // The root page of your application

            Title = "FirstApp";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Children = {
                    new Label {
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = "Welcome to Xamarin Forms!"
                    },
                    mRouteEntry,
                    mAddButton,
                    mDepartureListView
                }
            };
            

            
        }

        private void addDepartures()
        {
            mDepartures.Add(new Departure("5:55pm", 50585, 4));
        }

    }
}
