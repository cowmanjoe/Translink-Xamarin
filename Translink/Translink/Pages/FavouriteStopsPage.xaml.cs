using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            MessagingCenter.Send(this, "On Appearing"); 
        }
    }
}
