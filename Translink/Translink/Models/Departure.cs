﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    [ImplementPropertyChanged]
    public class Departure
    {
        public Departure(string time, int stopNumber, string routeNumber)
        {
            Time = time;
            StopNumber = stopNumber;
            RouteNumber = routeNumber;
        }

        public string Time
        {
            get; 
        }

        public int StopNumber
        {
            get; 
        }

        public string RouteNumber
        {
            get; 
        }

        public string AsString
        {
            get { return StopNumber + " [" + RouteNumber + "] " + Time; }
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Departure d = (Departure)obj;

            return Time == d.Time && StopNumber == d.StopNumber && RouteEquals(RouteNumber, d.RouteNumber); 

            

        }

        public override int GetHashCode()
        {
            return Time.GetHashCode() * RouteNumber.GetHashCode() * base.GetHashCode();
        }

        /*
         * Check equality of two route strings
         * ASSUMES: both routes are of the form <Optional-Letters><Numbers><Optional-Letters> 
        */
        public static bool RouteEquals(string r1, string r2)
        {
            string leadingLetters1 = TakeWhileLetter(r1);
            leadingLetters1 = leadingLetters1.ToUpper();

            string digits1 = TakeWhileDigit(r1.Substring(leadingLetters1.Length));

            string trailingLetters1 = TakeWhileLetter(r1.Substring(leadingLetters1.Length + digits1.Length));
            trailingLetters1 = trailingLetters1.ToUpper();


            string leadingLetters2 = TakeWhileLetter(r2);
            leadingLetters2 = leadingLetters2.ToUpper();

            string digits2 = TakeWhileDigit(r2.Substring(leadingLetters2.Length));

            string trailingLetters2 = TakeWhileLetter(r2.Substring(leadingLetters2.Length + digits2.Length));
            trailingLetters2 = trailingLetters2.ToUpper();

            int numbers1;
            int numbers2; 
            try
            {
                numbers1 = Convert.ToInt32(digits1);
                numbers2 = Convert.ToInt32(digits2); 
            }
            catch(FormatException)
            {
                throw new FormatException("The route was improperly formatted, there must be some numbers.");
            }

            if (leadingLetters1.Equals(leadingLetters2) &&
                Convert.ToInt32(digits1) == Convert.ToInt32(digits2) &&
                trailingLetters1.Equals(trailingLetters2))
                return true;
            return false;
        }
       

        private static string TakeWhileLetter(string s)
        {
            int i = 0;
            StringBuilder ans = new StringBuilder(); 
            while (s.Length > i && Char.IsLetter(s[i]))
            {
                ans.Append(s[i]);
                i++;
            }
            return ans.ToString(); 
        }

        private static string TakeWhileDigit(string s)
        {
            int i = 0;
            StringBuilder ans = new StringBuilder(); 
            while (s.Length > i && Char.IsDigit(s[i]))
            {
                ans.Append(s[i]); 
                i++; 
            }
            return ans.ToString(); 
        }
    }
}