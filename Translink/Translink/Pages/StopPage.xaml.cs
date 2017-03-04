using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translink.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopPage : BasePage
    {
        public StopPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var stopPageModel = BindingContext as StopPageModel;

            List<string> routes = stopPageModel.AvailableRoutes;

            foreach (string route in stopPageModel.AvailableRoutes)
            {
                RoutePicker.Items.Add(route);
            }
        }
    }
}
