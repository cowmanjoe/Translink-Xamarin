using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.PageModels;
using Xamarin.Forms;

namespace Translink.Pages
{
    public partial class FavouriteStopsPage : BasePage
    {
        public FavouriteStopsPage()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<FavouriteStopsPageModel>(
                this,
                "DeleteFavouritesPrompt",
                async (sender) => await DeleteFavouritesPrompt());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FavouriteStopsPageModel>(this, "DeleteFavouritesPrompt"); 
        }

        private async Task DeleteFavouritesPrompt()
        {
            bool delete = await DisplayAlert("", "Delete all favourite stops?", "Yes", "Cancel");
            
            if (delete)
            {
                MessagingCenter.Send(this, "DeleteFavourites"); 
            } 
        }
    }
}
