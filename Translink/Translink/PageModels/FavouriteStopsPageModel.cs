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

        public override void Init(object initData)
        {
            base.Init(initData);
            //await InitStopList();
            StopList.Add(new Translink.StopInfo(59484, "abcd"));
            StopList.Add(new Translink.StopInfo(59484, "abcd"));
            StopList.Add(new Translink.StopInfo(59484, "abcd"));
            StopList.Add(new Translink.StopInfo(59484, "abcd"));
            StopList.Add(new Translink.StopInfo(59484, "abcd"));

            //Stop stop = new Translink.Stop(new Translink.StopInfo { Name = "abcd", Number = 59434 }); 

            //StopList.Add(new Translink.Route("004", "NORTH", stop)); 
        }

        //async Task InitStopList()
        //{
        //    List<StopInfo> stopList = await mDataService.GetFavouriteStopInfos();
        //    StopList.Clear();
        //    StopList.Add(stopList[0]); 
        //    foreach (StopInfo si in stopList)
        //    {
        //        StopList.Add(si); 
        //    }
        //}
    }
}
