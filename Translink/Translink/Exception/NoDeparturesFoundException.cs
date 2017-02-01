using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink.Exception
{
    public class NoDeparturesFoundException : System.Exception
    {
        public NoDeparturesFoundException()
        {

        }

        public NoDeparturesFoundException(string message)
            :base(message)
        {

        }

       
    }
}
