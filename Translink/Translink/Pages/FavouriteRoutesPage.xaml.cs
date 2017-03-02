using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Translink.Pages
{
    public partial class FavouriteRoutesPage : BasePage
    {
        public FavouriteRoutesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send<FavouriteRoutesPage>(this, "On Appearing"); 
        }
    }
}
