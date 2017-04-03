using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public static class Util
    {
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
            
            try
            {
                Convert.ToInt32(digits1);
                Convert.ToInt32(digits2);
            }
            catch (FormatException)
            {
                throw new FormatException("The route was improperly formatted, there must be some numbers.");
            }

            return (leadingLetters1.Equals(leadingLetters2) &&
                    Convert.ToInt32(digits1) == Convert.ToInt32(digits2) &&
                    trailingLetters1.Equals(trailingLetters2));
        }

        /*
         * Checks whether routes are in the form <Optional-Letters><Numbers><Optional-Letters>
        */
        public static bool IsValidRoute(string r)
        {
            string leadingLetters = TakeWhileLetter(r);
            string digits = TakeWhileDigit(r.Substring(leadingLetters.Length));
            string trailingLetters = TakeWhileLetter(r.Substring(leadingLetters.Length + digits.Length));

            return digits.Length != 0 && r.Length == leadingLetters.Length + digits.Length + trailingLetters.Length; 
        }


        private static string TakeWhileLetter(string s)
        {
            int i = 0;
            StringBuilder ans = new StringBuilder();
            while (s.Length > i && char.IsLetter(s[i]))
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
            while (s.Length > i && char.IsDigit(s[i]))
            {
                ans.Append(s[i]);
                i++;
            }
            return ans.ToString();
        }

    }
}
