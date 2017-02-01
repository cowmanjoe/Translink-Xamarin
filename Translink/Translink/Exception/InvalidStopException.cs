using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Exception
{
    public class InvalidStopException : System.Exception 
    {
        public InvalidStopException()
        {

        }

        public InvalidStopException(string message) 
            : base(message)
        {

        }

        public InvalidStopException(int stop)
            : base("Stop number " + stop + " is an invalid stop number.")
        {
            
        }
    }
}
