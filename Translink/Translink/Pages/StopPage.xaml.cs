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
            MessagingCenter.Subscribe<StopPageModel>(this, "RefreshRoutes", (sender) => RefreshRoutes(sender));
            DepartureList.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "OnAppearing"); 
        }

        private void RefreshRoutes(StopPageModel pageModel)
        {
            base.OnBindingContextChanged();
            

            List<string> routes = pageModel.AvailableRoutes;

            RoutePicker.Items.Clear(); 
            foreach (string route in pageModel.AvailableRoutes)
            {
                RoutePicker.Items.Add(route);
            }
        }
    }
}
