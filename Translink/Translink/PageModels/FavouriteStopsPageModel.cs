using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;

namespace Translink.PageModels
{
    public class FavouriteStopsPageModel : FreshBasePageModel
    {
        private IFavouritesDataService mDataService;

        public ObservableCollection<StopInfo> StopList { get; set; }
       
        public FavouriteStopsPageModel(IFavouritesDataService dataService)
        {
            mDataService = dataService;
            StopList = new ObservableCollection<StopInfo>(); 
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            await InitStopList();
        }

        async Task InitStopList()
        {
            List<StopInfo> stopList = await mDataService.GetFavouriteStopInfos();
            StopList.Clear();
            foreach (StopInfo si in stopList)
            {
                StopList.Add(si);
            }
        }
    }
}
