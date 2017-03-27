using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Pages;
using Translink.Services;
using Xamarin.Forms;
using Translink.Models;
using System.Diagnostics;

namespace Translink.PageModels
{
    public class FavouriteStopsPageModel : FreshBasePageModel
    {
        private IFavouritesDataService mDataService;

        public ObservableCollection<StopInfo> StopList { get; set; }

        public StopInfo SelectedStopInfo
        {
            get { return null; }
            set
            {
                CoreMethods.PushPageModel<StopPageModel>(value);
                RaisePropertyChanged(); 
            }
        }
       
        public FavouriteStopsPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            StopList = new ObservableCollection<StopInfo>();
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<FavouriteStopsPage>(
                this,
                "DeleteFavourites",
                async (page) => await DeleteFavourites());
            MessagingCenter.Subscribe<FavouriteStopsPage, StopInfo>(
                this,
                "DeleteFavourite",
                async (page, stopInfo) => await DeleteFavourite(stopInfo));
            await RefreshStopList(); 
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            MessagingCenter.Unsubscribe<FavouriteStopsPage>(this, "DeleteFavourites");
            MessagingCenter.Unsubscribe<FavouriteStopsPage, StopInfo>(this, "DeleteFavourite"); 
        }
        

        async Task RefreshStopList()
        {
            List<StopInfo> stopList = await mDataService.GetFavouriteStopInfos();
            StopList.Clear();
            foreach (StopInfo si in stopList)
            {
                StopList.Add(si);
            }
        }

        public Command SendDeleteFavouritesPrompt
        {
            get
            {
                return new Command(() => 
                {
                    MessagingCenter.Send(this, "DeleteFavouritesPrompt");
                }); 
            }
        }

        public Command SendDeleteFavourite
        {
            get
            {
                return new Command((sender) => 
                {
                    Button button = sender as Button;
                    Debug.WriteLine(button.Parent); 
                }); 
            }
        }

        private async Task DeleteFavourites()
        {
            await mDataService.ClearFavouriteStops();
            StopList.Clear();
        }

        private async Task DeleteFavourite(StopInfo stopInfo)
        {
            await mDataService.RemoveFavouriteStop(stopInfo.Number);
            await RefreshStopList(); 
        }
    }
}
