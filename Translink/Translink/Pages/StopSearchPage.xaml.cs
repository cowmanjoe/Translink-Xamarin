using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.PageModels;
using Xamarin.Forms;

namespace Translink.Pages
{
    public partial class StopSearchPage : BasePage
    {
        public StopSearchPage()
        {
            MessagingCenter.Subscribe<StopSearchPageModel, Alert>(this, "Display Alert", async (pageModel, alert) => await DisplayAlert(alert));
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StopListView.SelectedItem = null; 
        }
    }
}
