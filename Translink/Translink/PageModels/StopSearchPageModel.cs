using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Services;
using Translink.Exception; 
using Xamarin.Forms;


namespace Translink.PageModels
{
    public class StopSearchPageModel : FreshMvvm.FreshBasePageModel
    {
        private IDepartureDataService mDataService;

        public ObservableCollection<Departure> DepartureList { get; set; }

        public int StopNumber { get; set; }

        public string RouteNumber { get; set; }

        public Boolean RouteToggled { get; set; }

        public StopSearchPageModel(IDepartureDataService dataService)
        {
            mDataService = dataService;
        }

        public async override void Init(object initData)
        {
            base.Init(initData);
            DepartureList = new ObservableCollection<Departure>();
            StopNumber = 50586;
            RouteNumber = "004";
            RouteToggled = false;
        }

        public Command SearchDepartures
        {
            get
            {
                return new Command(async () =>
                {
                    List<Departure> departureList = new List<Departure>();
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
                        Alert alert = new Alert("Invalid Stop", "Use a valid 5 digit stop number.", "OK");
                        MessagingCenter.Send<FreshMvvm.FreshBasePageModel, Alert>(this, "Display Alert", alert);

                    }
                    catch (TranslinkAPIErrorException e)
                    {
                        Alert alert = new Alert("API Error", "An error occurred in the Translink API", "OK");
                        MessagingCenter.Send<FreshMvvm.FreshBasePageModel, Alert>(this, "Display Alert", alert);
                    }
                });
            }
        }

        public Command RefreshDepartures
        {
            get
            {
                return new Command(async () =>
                {
                    await mDataService.RefreshDepartures();
                    List<Departure> departureList = mDataService.GetDepartures();

                    DepartureList.Clear();

                    foreach (Departure d in departureList)
                    {
                        DepartureList.Add(d);
                        Debug.WriteLine("Departure added: " + d.AsString);
                    }
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
