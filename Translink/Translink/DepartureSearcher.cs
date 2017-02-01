using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translink.Exception; 

namespace Translink
{
    public class DepartureSearcher
    {

        // List that populates the ListView of departures 
        ObservableCollection<Departure> mDepartures;

        // List of stop/routes pairs of searches that have been made
        // if the list is empty then all stops were searched for 
        Dictionary<int, List<string>> mSearches;

        DepartureDataFetcher mDepartureDataFetcher;


        public ObservableCollection<Departure> Departures
        {
            get { return mDepartures; }
        }
        
        public DepartureSearcher()
        {
            mDepartures = new ObservableCollection<Departure>();

            mDepartureDataFetcher = new DepartureDataFetcher();
            mSearches = new Dictionary<int, List<string>>();
            
        }
        
        
        /**
         * Searches for departures using Translink API and adds them to mDepartures 
         * stop: the stop number 
         */ 
        public async Task SearchAndAddDepartures(int stop)
        {
            
            bool alreadySearched = false;

            List<string> routeList;
            if (mSearches.TryGetValue(stop, out routeList))
                alreadySearched = routeList.Count == 0;

            if (!alreadySearched)
            {
                List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(stop);
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
        public async Task SearchAndAddDepartures(int stop, string route)
        {
            
            bool alreadySearched = false;
            List<string> routeList;
            if (mSearches.TryGetValue(stop, out routeList))
            {
                foreach (string r in routeList)
                {
                    if (routeEquals(r, route)) alreadySearched = true;
                }
                if (routeList.Count == 0) alreadySearched = true;
            }

            if (!alreadySearched)
            {
                List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(stop, route);
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
                    List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(s);
                    foreach (Departure d in departures)
                        mDepartures.Add(d);
                }
                else {
                    foreach (string r in routeList)
                    {
                        List<Departure> departures = await mDepartureDataFetcher.fetchDepartures(s, r);
                        foreach (Departure d in departures)
                            mDepartures.Add(d); 
                    }
                }
            }
        }




        /*
         * Check equality of two route strings
         * ASSUMES: both routes are of the form <Optional-Letters><Numbers><Optional-Letters> 
        */
        private bool routeEquals(string r1, string r2)
        {
            string leadingLetters1 = "";
            int numbersIndex1 = 0; 

            int i1;
            //find index of first digit of r1 
            for (i1 = 0; i1 < r1.Length - 1; i1++)
            {
                if (char.IsLetter(r1[i1]) && char.IsDigit(r1[i1 + 1]))
                {
                    leadingLetters1 = r1.Substring(0, i1 + 1);
                    numbersIndex1 = i1 + 1;
                    break;
                }
            }

            // find index of trailing letters of r1
            for (; i1 < r2.Length - 1; i1++)
            {
                if (char.IsDigit(r1[i1]) && char.IsLetter(r1[i1 + 1]))
                {

                    break;
                }
            }
            string numbers1 = r1.Substring(numbersIndex1, i1 + 1);
            string trailingLetters1 = i1 + 1 < r1.Length ? r1.Substring(i1 + 1) : "";


            string leadingLetters2 = "";
            int numbersIndex2 = 0; 

            int i2;
            //find index of first digit of r2 
            for (i2 = 0; i2 < r2.Length - 2; i2++)
            {
                if (char.IsLetter(r2[i2]) && char.IsDigit(r2[i2 + 2]))
                {
                    leadingLetters2 = r2.Substring(0, i2 + 1);
                    numbersIndex2 = i2 + 1;
                    break;
                }
            }

            // find index of trailing letters of r2
            for (; i2 < r2.Length - 2; i2++)
            {
                if (char.IsDigit(r2[i2]) && char.IsLetter(r2[i2 + 2]))
                {

                    break;
                }
            }
            string numbers2 = r2.Substring(numbersIndex2, i2 + 1);
            string trailingLetters2 = i2 + 1 < r2.Length ? r2.Substring(i2 + 1) : "";
            

            if (leadingLetters1.Equals(leadingLetters2) &&
                Convert.ToInt32(numbers1) == Convert.ToInt32(numbers2) &&
                trailingLetters1.Equals(trailingLetters2))
                return true;
            return false;
        }


        

        private int numDigits(int x)
        {
            return (int)Math.Ceiling(Math.Log10(x)); 
        }
    }
}
