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
        private IDepartureDataService mDataService;

        public ObservableCollection<Departure> DepartureList { get; set; }

        public int StopNumber { get; set; }
        
        public string RouteNumber { get; set; }

        public bool RouteToggled { get; set; } 

        public bool IsBusy { get; private set; }

        public StopSearchPageModel(IDepartureDataService dataService)
        {
            mDataService = dataService; 
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
                            await mDataService.SearchDepartures(StopNumber, RouteNumber);
                            departureList = mDataService.GetDepartures();
                        }
                        else
                        {
                            await mDataService.SearchDepartures(StopNumber);
                            departureList = mDataService.GetDepartures();
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
                    await mDataService.RefreshDepartures();
                    List<Departure> departureList = mDataService.GetDepartures();

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
                    mDataService.ClearDepartures();
                    DepartureList.Clear(); 
                });
            }
        }
        
    }
}
