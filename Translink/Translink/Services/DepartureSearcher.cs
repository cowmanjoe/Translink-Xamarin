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
        
        
        public DepartureSearcher()
        {
            mDepartures = new List<Departure>();

            
            mSearches = new Dictionary<int, List<string>>();
            
        }

        #region IDepartureDataService implementation

        /**
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         */
        public async Task SearchDepartures(int stop)
        {
            
            bool alreadySearched = false;

            List<string> routeList;
            if (mSearches.TryGetValue(stop, out routeList))
                alreadySearched = routeList.Count == 0;

            if (!alreadySearched)
            {
                List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(stop);
                foreach (Departure d in departures)
                {
                    mDepartures.Add(d);
                }

                if (mSearches.ContainsKey(stop)) mSearches.Remove(stop);
                mSearches.Add(stop, new List<string>());
            }
        }

        /** 
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         * route: the route number 
         */ 
        public async Task SearchDepartures(int stop, string route)
        {
            
            bool alreadySearched = false;
            List<string> routeList;
            if (mSearches.TryGetValue(stop, out routeList))
            {
                foreach (string r in routeList)
                {
                    if (Departure.RouteEquals(r, route)) alreadySearched = true;
                }
                if (routeList.Count == 0) alreadySearched = true;
            }

            if (!alreadySearched)
            {
                List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(stop, route);
                foreach (Departure d in departures)
                {
                    mDepartures.Add(d);
                }


                if (mSearches.TryGetValue(stop, out routeList))
                {
                    if (routeList.Count > 0)
                    {
                        routeList.Add(route);
                    }
                }
                else
                {
                    routeList = new List<string>();
                    routeList.Add(route);
                    mSearches.Add(stop, routeList);
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

            foreach (int s in mSearches.Keys)
            {
                List<string> routeList;
                mSearches.TryGetValue(s, out routeList); 
                if (routeList.Count == 0)
                {
                    List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(s);
                    foreach (Departure d in departures)
                        mDepartures.Add(d);
                }
                else {
                    foreach (string r in routeList)
                    {
                        List<Departure> departures = await DepartureDataFetcher.Instance.fetchDepartures(s, r);
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
