using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Exception;
using Translink.Services;

namespace Translink.Services
{
    public class DepartureSearcher : IDepartureDataService
    {

        // List that populates the ListView of departures 
        private readonly List<Departure> mDepartures;

        // List of stop/routes pairs of searches that have been made
        // if the list is empty then all stops were searched for 
        private readonly Dictionary<int, List<string>> mSearches;

        private readonly List<Stop> mStops; 
        
        
        public DepartureSearcher()
        {
            mDepartures = new List<Departure>();

            mStops = new List<Stop>(); 
            mSearches = new Dictionary<int, List<string>>();
            
        }

        /*
         * Gets stop with given number in mStops if it is contained in it, null otherwise
         */
        private Stop GetStop(int stopNo)
        {
            foreach (Stop s in mStops)
            {
                if (s.Number == stopNo)
                    return s; 
            }
            return null; 
        }

        #region IDepartureDataService implementation

        /**
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         */
        public async Task SearchDepartures(int stopNo)
        {
            
            bool alreadySearched = false;

            List<string> routeList;
            if (mSearches.TryGetValue(stopNo, out routeList))
                alreadySearched = routeList.Count == 0;

            if (!alreadySearched)
            {
                Stop stop = GetStop(stopNo);
                if (stop == null)
                {
                    stop = await StopDataFetcher.Instance.FetchStopWithDepartures(stopNo);
                    mStops.Add(stop); 
                }

                List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(stop); 
                foreach (Departure d in departures)
                {
                    mDepartures.Add(d);
                }

                if (mSearches.ContainsKey(stopNo)) mSearches.Remove(stopNo);
                mSearches.Add(stopNo, new List<string>());
            }
        }

        /** 
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         * route: the route number 
         */ 
        public async Task SearchDepartures(int stopNo, string routeNo)
        {
            
            bool alreadySearched = false;
            List<string> routeList;
            if (mSearches.TryGetValue(stopNo, out routeList))
            {
                foreach (string r in routeList)
                {
                    if (Departure.RouteEquals(r, routeNo)) alreadySearched = true;
                }
                if (routeList.Count == 0) alreadySearched = true;
            }

            if (!alreadySearched)
            {
                Stop stop = GetStop(stopNo);
                if (stop == null)
                {
                    stop = await StopDataFetcher.Instance.FetchStopWithDepartures(stopNo);
                    mStops.Add(stop); 
                }

                List<Departure> departures = stop.GetRouteDepartures(routeNo); 
                foreach (Departure d in departures)
                {
                    mDepartures.Add(d);
                }


                if (mSearches.TryGetValue(stopNo, out routeList))
                {
                    if (routeList.Count > 0)
                    {
                        routeList.Add(routeNo);
                    }
                }
                else
                {
                    routeList = new List<string>();
                    routeList.Add(routeNo);
                    mSearches.Add(stopNo, routeList);
                }
            }
            Debug.WriteLine("Contents of mSearches:");
            foreach (int k in mSearches.Keys)
            {
                Debug.WriteLine("  K = " + k + ": ");
                List<string> routes;
                if (mSearches.TryGetValue(k, out routes))
                {
                    foreach (string r in routes)
                    {
                        Debug.WriteLine("    R = " + r);
                    }
                }
            }

        }

        /** 
         * Clears the departures and searches list 
         */ 
        public void ClearDepartures()
        {
            mDepartures.Clear();
            mSearches.Clear();
        }


        /** 
         * Refreshes the departures using the searches that have already been made 
         */ 
        public async Task RefreshDepartures()
        {
            mDepartures.Clear();

            foreach (int stopNo in mSearches.Keys)
            {
                List<string> routeList;
                mSearches.TryGetValue(stopNo, out routeList); 
                if (routeList.Count == 0)
                {
                    Stop stop = GetStop(stopNo);
                    if (stop == null)
                        throw new System.Exception("Failed to find a stop in memory even though it should be saved.");

                    List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(stop);
                    foreach (Departure d in departures)
                        mDepartures.Add(d);
                }
                else {
                    foreach (string r in routeList)
                    {
                        Stop stop = GetStop(stopNo);
                        if (stop == null)
                            throw new System.Exception("Failed to find a stop in memory even though it should be saved.");

                        List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(stop, r);
                        foreach (Departure d in departures)
                            mDepartures.Add(d); 
                    }
                }
            }
        }

        public List<Departure> GetDepartures()
        {
            return mDepartures; 
        }

        #endregion
    }
}
