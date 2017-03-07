using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Translink
{
    public class RouteViewCell : ViewCell
    {
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create("Name", typeof(string), typeof(RouteViewCell), ""); 

        public string Name
        {
            get { return (string)GetValue(NameProperty);  }
            set { SetValue(NameProperty, value); }
        }

        public static readonly BindableProperty RouteNumberProperty =
            BindableProperty.Create("RouteNumber", typeof(string), typeof(RouteViewCell), "");

        public string RouteNumber
        {
            get { return (string)GetValue(RouteNumberProperty); }
            set { SetValue(RouteNumberProperty, value); }
        }

        public static readonly BindableProperty StopNameProperty =
            BindableProperty.Create("StopName", typeof(string), typeof(RouteViewCell), "");

        public string StopName
        {
            get { return (string)GetValue(StopNameProperty); }
            set { SetValue(StopNameProperty, value); }
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create("Direction", typeof(string), typeof(RouteViewCell), "");

        public string Direction
        {
            get { return (string)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value + "BOUND"); }
        }
    }
}
