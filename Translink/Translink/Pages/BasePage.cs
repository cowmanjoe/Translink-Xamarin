using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Translink.Pages
{
    public class BasePage : ContentPage
    {
        public BasePage()
        {
            MessagingCenter.Subscribe<FreshMvvm.FreshBasePageModel, Alert>(this, "Display Alert", async (pageModel, alert) => await DisplayAlert(alert));
        }

        protected async Task DisplayAlert(Alert alert)
        {
            await DisplayAlert(alert.Title, alert.Message, alert.Cancel);
        }
    }
}
