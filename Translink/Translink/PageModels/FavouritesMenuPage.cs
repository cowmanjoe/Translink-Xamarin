using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FreshMvvm;
using Translink.Pages;

namespace Translink.PageModels
{
    public class FavouritesMenuPage : FreshBasePageModel
    {
        
        public Command ShowStops
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<FavouriteStopsPageModel>();
                });
            }
        }

        public Command ShowRoutes
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<FavouriteRoutesPageModel>();
                });
            }
        }
    }
}
