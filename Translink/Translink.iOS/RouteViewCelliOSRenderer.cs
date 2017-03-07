using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Translink;
using Translink.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RouteViewCell), typeof(RouteViewCelliOSRenderer))]
namespace Translink.iOS
{
    
    public class RouteViewCelliOSRenderer : ViewCellRenderer
    {
        RouteiOSViewCell cell;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var routeCell = (RouteViewCell)item;

            cell = reusableCell as RouteiOSViewCell;
            if (cell == null)
                cell = new RouteiOSViewCell(item.GetType().FullName, routeCell);
            else
                cell.RouteViewCell.PropertyChanged -= OnRouteViewCellPropertyChanged;

            routeCell.PropertyChanged += OnRouteViewCellPropertyChanged;
            cell.UpdateCell(routeCell);
            return cell;
        }

        void OnRouteViewCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var routeCell = (RouteViewCell)sender; 
            if (e.PropertyName == RouteViewCell.NameProperty.PropertyName)
            {
                cell.Name = routeCell.Name; 
            }
            else if (e.PropertyName == RouteViewCell.RouteNumberProperty.PropertyName)
            {
                cell.RouteLabel.Text = routeCell.RouteNumber; 
            }
            else if (e.PropertyName == RouteViewCell.DirectionProperty.PropertyName)
            {
                cell.DirectionLabel.Text = routeCell.Direction; 
            }
            else if (e.PropertyName == RouteViewCell.StopNameProperty.PropertyName)
            {
                cell.StopNameLabel.Text = routeCell.StopName; 
            }
        }
    }


}
