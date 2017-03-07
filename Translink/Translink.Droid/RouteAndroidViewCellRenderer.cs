using Translink;
using Xamarin.Forms;
using Translink.Droid;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(RouteViewCell), typeof(RouteAndroidViewCellRenderer))]

namespace Translink.Droid
{
    public class RouteAndroidViewCellRenderer : ViewCellRenderer
    {
        RouteAndroidViewCell cell; 

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var routeCell = (RouteViewCell)item; 
            
            cell = convertView as RouteAndroidViewCell;
            if (cell == null)
            {
                cell = new RouteAndroidViewCell(context, routeCell); 
            }
            else
            {
                cell.RouteViewCell.PropertyChanged -= OnRouteCellPropertyChanged; 
            }


            cell.UpdateCell(routeCell); 
            return cell; 
        }


        void OnRouteCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var routeCell = (RouteViewCell)sender;
            if (e.PropertyName == RouteViewCell.NameProperty.PropertyName)
            {
                cell.Name = routeCell.Name;
            }
            else if (e.PropertyName == RouteViewCell.RouteNumberProperty.PropertyName)
            {
                cell.RouteNumberTextView.Text = routeCell.RouteNumber;
            }
            else if (e.PropertyName == RouteViewCell.DirectionProperty.PropertyName)
            {
                cell.DirectionTextView.Text = routeCell.Direction;
            }
            else if (e.PropertyName == RouteViewCell.StopNameProperty.PropertyName)
            {
                cell.StopNameTextView.Text = routeCell.StopName; 
            }
        }
    }
}