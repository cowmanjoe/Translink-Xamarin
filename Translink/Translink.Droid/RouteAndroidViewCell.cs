using Android.App;
using Android.Content;
using Android.Widget;
using Xamarin.Forms;

namespace Translink.Droid
{
    internal class RouteAndroidViewCell : LinearLayout, INativeElementView
    {
        public string Name { get; set; }
        public TextView RouteNumberTextView { get; set; }
        public TextView DirectionTextView { get; set; }
        public TextView StopNameTextView { get; set; }

        public RouteViewCell RouteViewCell { get; private set; }
        public Element Element => RouteViewCell;

        public RouteAndroidViewCell(Context context, RouteViewCell cell) : base(context)
        {
            RouteViewCell = cell;

            var view = (context as Activity).LayoutInflater.Inflate(Resource.Layout.RouteAndroidViewCell, null);
            RouteNumberTextView = view.FindViewById<TextView>(Resource.Id.RouteNumberText);
            DirectionTextView = view.FindViewById<TextView>(Resource.Id.DirectionText);
            StopNameTextView = view.FindViewById<TextView>(Resource.Id.StopNameText);

            AddView(view);
        }

        public void UpdateCell(RouteViewCell cell)
        {
            Name = cell.Name;
            RouteNumberTextView.Text = cell.RouteNumber;
            DirectionTextView.Text = cell.Direction;
            StopNameTextView.Text = cell.StopName;
        }
    }
}