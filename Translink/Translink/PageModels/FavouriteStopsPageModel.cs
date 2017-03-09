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
            MessagingCenter.Subscribe<FavouriteStopsPage>(
                this,
                "On Appearing",
                async (page) => await RefreshStopList()); 
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            await RefreshStopList();
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

        public Command ClearFavouriteStops
        {
            get
            {
                return new Command(async () => {
                    await mDataService.ClearFavouriteStops();
                    StopList.Clear(); 
                    }); 
            }
        }
    }
}
