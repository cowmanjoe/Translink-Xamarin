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

namespace Translink.PageModels
{
    public class StopSearchPageModel : FreshMvvm.FreshBasePageModel
    {
        private IStopDataService mStopDataService;
        private IFavouritesDataService mFavouritesDataService; 

        public ObservableCollection<Departure> DepartureList { get; set; }

        public int StopNumber { get; set; }
        
        public string RouteNumber { get; set; }

        public bool RouteToggled { get; set; } 

        public bool IsBusy { get; private set; }

        public StopSearchPageModel(IStopDataService stopDataService, IFavouritesDataService favouritesDataService)
        {
            mStopDataService = stopDataService;
            mFavouritesDataService = favouritesDataService; 
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            DepartureList = new ObservableCollection<Departure>();
            StopNumber = 50586;
            RouteNumber = "004"; 
            RouteToggled = false;
            IsBusy = false; 
        }

        public Command SearchDepartures
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true; 
                    List<Departure> departureList;
                    try
                    {
                        if (RouteToggled)
                        {
                            await mStopDataService.SearchDepartures(StopNumber, RouteNumber);
                            departureList = mStopDataService.GetDepartures();
                        }
                        else
                        {
                            await mStopDataService.SearchDepartures(StopNumber);
                            departureList = mStopDataService.GetDepartures();
                        }

                        DepartureList.Clear();
                        foreach (Departure d in departureList)
                        {
                            DepartureList.Add(d);
                            Debug.WriteLine("Departure added: " + d.AsString);
                        }
                    }
                    catch (InvalidStopException e)
                    {
                        Alert alert = new Alert("Invalid Stop", e.Message, "OK");
                        MessagingCenter.Send<StopSearchPageModel, Alert>(this, "Display Alert", alert);
                    }
                    catch (TranslinkAPIErrorException e)
                    {
                        Alert alert = new Alert("API Error", "An error occurred in the Translink API.", "OK");
                        MessagingCenter.Send<StopSearchPageModel, Alert>(this, "Display Alert", alert);
                    }
                    catch (System.Exception e)
                    {
                        Alert alert = new Alert("Error", "Something went wrong with that request.", "OK");
                        MessagingCenter.Send<StopSearchPageModel, Alert>(this, "Display Alert", alert); 
                    }

                    IsBusy = false; 
                });
            }
        }

        public Command RefreshDepartures
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true; 
                    await mStopDataService.RefreshDepartures();
                    List<Departure> departureList = mStopDataService.GetDepartures();

                    DepartureList.Clear(); 

                    foreach (Departure d in departureList)
                    {
                        DepartureList.Add(d);
                        Debug.WriteLine("Departure added: " + d.AsString);
                    }

                    IsBusy = false; 
                });
            }
        }

        public Command ClearDepartures
        {
            get
            {
                return new Command(() =>
                {
                    mStopDataService.ClearDepartures();
                    DepartureList.Clear(); 
                });
            }
        }
        
        public Command AddStopToFavourites
        {
            get
            {
                return new Command (() =>
                {
                    
                })
            }
        }
    }
}
