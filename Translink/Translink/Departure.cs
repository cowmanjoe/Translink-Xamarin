using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public struct Departure
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

        /*
         * Check equality of two route strings
         * ASSUMES: both routes are of the form <Optional-Letters><Numbers><Optional-Letters> 
        */
        public static bool RouteEquals(string r1, string r2)
        {
            string leadingLetters1;
            string leadingLetters2;
            string digits1 = "";
            string digits2 = "";
            string trailingLetters1 = "";
            string trailingLetters2 = "";

            

            int ll1Length;
            leadingLetters1 = TakeWhileLetter(r1, out ll1Length); 
            leadingLetters1 = leadingLetters1.ToUpper();

            int n1Length = 0; 
            if (ll1Length < r1.Length)
                digits1 = TakeWhileDigit(r1.Substring(ll1Length), out n1Length);
            
            if (ll1Length + n1Length < r1.Length)
            {
                trailingLetters1 = TakeWhileLetter(r1.Substring(ll1Length + n1Length)); 
                trailingLetters1 = trailingLetters1.ToUpper();
            }

            int ll2Length;
            leadingLetters2 = TakeWhileLetter(r2, out ll2Length);
            leadingLetters2 = leadingLetters2.ToUpper();

            int n2Length = 0;
            if (ll2Length < r2.Length)
                digits2 = TakeWhileDigit(r2.Substring(ll2Length), out n2Length);

            if (ll2Length + n2Length < r2.Length)
            {
                trailingLetters2 = TakeWhileLetter(r2.Substring(ll2Length + n2Length));
                trailingLetters2 = trailingLetters2.ToUpper();
            }


            int numbers1;
            int numbers2; 
            try
            {
                numbers1 = Convert.ToInt32(digits1);
                numbers2 = Convert.ToInt32(digits2); 
            }
            catch(FormatException e)
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
            int temp;
            return TakeWhileLetter(s, out temp); 
        }

        private static string TakeWhileDigit(string s)
        {
            int temp;
            return TakeWhileDigit(s, out temp); 
        }

        private static string TakeWhileLetter(string s, out int length)
        {
            length = 0;
            string ans = ""; 
            while (s.Length > length && Char.IsLetter(s[length]))
            {
                ans += s[length];
                length++;
            }
            return ans; 
        }

        private static string TakeWhileDigit(string s, out int length)
        {
            length = 0;
            string ans = ""; 
            while (s.Length > length && Char.IsDigit(s[length]))
            {
                ans += s[length];
                length++; 
            }
            return ans; 
        }
    }
}
