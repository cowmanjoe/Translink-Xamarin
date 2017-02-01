using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Exception
{
    class TranslinkAPIErrorException : System.Exception
    {
        public TranslinkAPIErrorException()
        {

        }

        public TranslinkAPIErrorException(string message) 
            : base(message)
        {

        }

        public TranslinkAPIErrorException(int code) 
            : base("Translink returned an error. Code=" + code + ".")
        {

        }

    }
}
