using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;

namespace Translink.iOS
{
    internal class RouteiOSViewCell : UITableViewCell, INativeElementView
    {
        public string Name { get; set; }

        public UILabel RouteLabel { get; set; }
        public UILabel StopNameLabel { get; set; }
        public UILabel DirectionLabel { get; set; }

        public RouteViewCell RouteViewCell { get; private set; }
        public Element Element => RouteViewCell;

        public RouteiOSViewCell(string cellId, RouteViewCell cell) : base(UITableViewCellStyle.Default, cellId)
        { 
            RouteViewCell = cell;

            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            ContentView.BackgroundColor = UIColor.FromRGB(255, 255, 224);

            DirectionLabel = new UILabel()
            {
                Font = UIFont.FromName("Cochin-BoldItalic", 50f),
                TextColor = UIColor.FromRGB(127, 51, 0),
                BackgroundColor = UIColor.Clear
            };

            RouteLabel = new UILabel()
            {
                Font = UIFont.FromName("Cochin-BoldItalic", 50f),
                TextColor = UIColor.FromRGB(127, 51, 0),
                BackgroundColor = UIColor.Clear
            };

            StopNameLabel = new UILabel()
            {
                Font = UIFont.FromName("AmericanTypewriter", 12f),
                TextColor = UIColor.FromRGB(38, 127, 0),
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };

            ContentView.Add(RouteLabel);
            ContentView.Add(StopNameLabel);
            ContentView.Add(DirectionLabel);
        }

        public void UpdateCell(RouteViewCell cell)
        {
            Name = cell.Name; 
            RouteLabel.Text = cell.RouteNumber;
            StopNameLabel.Text = cell.StopName;
            DirectionLabel.Text = cell.Direction; 
        }
        

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            //TODO: make these values correct!
            RouteLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
            StopNameLabel.Frame = new CGRect(100, 18, 100, 20);
            DirectionLabel.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
        }
    }
}
