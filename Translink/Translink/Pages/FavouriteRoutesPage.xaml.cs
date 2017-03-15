using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.PageModels;
using Xamarin.Forms;

namespace Translink.Pages
{
    public partial class FavouriteRoutesPage : BasePage
    {
        public FavouriteRoutesPage()
        {
            InitializeComponent();
            RouteList.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }


        private async Task DeleteFavouritesPrompt()
        {
            bool delete = await DisplayAlert("", "Delete all favourite routes?", "Yes", "Cancel");

            if (delete)
                MessagingCenter.Send(this, "DeleteFavourites");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<FavouriteRoutesPageModel>(this, "DeleteFavouritesPrompt", async (sender) => await DeleteFavouritesPrompt());

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FavouriteRoutesPageModel>(this, "DeleteFavouritesPrompt"); 
        }
    }
}
