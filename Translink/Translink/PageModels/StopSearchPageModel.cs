using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Exception;
using Translink.Services;
using Xamarin.Forms;
using Translink.Models;

namespace Translink.PageModels
{
    public class StopSearchPageModel : FreshMvvm.FreshBasePageModel
    {
        private IDepartureDataService mDepartureDataService;
        private IFavouritesDataService mFavouritesDataService;
        private IStopDataService mStopDataService; 

        public ObservableCollection<StopInfo> StopList { get; private set; }

        public ObservableCollection<Departure> DepartureList { get; set; }

        public int StopNumber { get; set; }
        
        public string RouteNumber { get; set; }

        public bool RouteToggled { get; set; } 

        public bool IsBusy { get; private set; }

        public StopInfo SelectedStop
        {
            get
            {
                return null; 
            }
            set
            {
                if (value != null)
                {
                    CoreMethods.PushPageModel<StopPageModel>(value);
                    RaisePropertyChanged();
                } 
            }
        }

        public StopSearchPageModel(IDepartureDataService departureDataService, IFavouritesDataService favouritesDataService, IStopDataService stopDataService)
        {
            mDepartureDataService = departureDataService;
            mFavouritesDataService = favouritesDataService;
            mStopDataService = stopDataService; 
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            DepartureList = new ObservableCollection<Departure>();
            StopList = new ObservableCollection<StopInfo>(); 
            StopNumber = 50586;
            RouteNumber = "004"; 
            RouteToggled = false;
            IsBusy = false; 
        }
        
        public Command SearchStop
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        StopInfo stopInfo = await mStopDataService.FetchStopInfo(StopNumber);
                        await CoreMethods.PushPageModel<StopPageModel>(stopInfo);
                        RaisePropertyChanged();
                    }
                    catch (InvalidStopException e)
                    {
                        Alert alert = new Alert("Invalid Stop", e.Message, "OK");
                        MessagingCenter.Send(this, "Display Alert", alert); 
                    }
                    catch (System.Exception e)
                    {
                        Alert alert = new Alert("Error", "Something went wrong with that request.", "OK");
                        MessagingCenter.Send(this, "Display Alert", alert); 
                    }
                });
            }
        }
        

        public Command RefreshStops
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    try
                    {
                        List<StopInfo> stopInfos = await mStopDataService.FetchStopInfosAroundMe();

                        StopList.Clear();
                        foreach (StopInfo si in stopInfos)
                        {
                            StopList.Add(si);
                        }
                    }
                    catch (LocationException e)
                    {
                        Alert alert;
                        if (e.GeolocationUnavailable)
                        {
                            alert = new Alert("Error", "Location services are unavailable.", "OK");
                        }
                        else if (e.GeolocationDisabled)
                        {
                            alert = new Alert("Error", "Location services are disabled.", "OK");
                        }
                        else
                        {
                            alert = new Alert("Error", "Something went wrong when finding your location.", "OK");
                        }
                        MessagingCenter.Send(this, "Display Alert", alert); 
                    }

                    IsBusy = false; 
                });
            }
        }
       
        
        public Command AddStopToFavourites
        {
            get
            {
                return new Command(async () =>
               {
                   try
                   {
                       StopInfo stopInfo = await mStopDataService.FetchStopInfo(StopNumber);
                       await mFavouritesDataService.AddFavouriteStop(stopInfo);
                   }
                   catch (InvalidStopException e)
                   {
                       Alert alert = new Alert("Invalid Stop", e.Message, "OK");
                       MessagingCenter.Send(this, "Display Alert", alert);
                   }
                   catch (TranslinkAPIErrorException e)
                   {
                       Alert alert = new Alert("API Error", "An error occurred in the Translink API.", "OK");
                       MessagingCenter.Send(this, "Display Alert", alert);
                   }
                   catch (System.Exception e)
                   {
                       Alert alert = new Alert("Error", "Something went wrong with that request.", "OK");
                       MessagingCenter.Send(this, "Display Alert", alert);
                   }
               });
            }
        }
    }
}
