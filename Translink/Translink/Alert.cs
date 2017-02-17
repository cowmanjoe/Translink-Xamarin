using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translink
{
    public struct Alert
    {
        public readonly string Title;
        public readonly string Message;
        public readonly string Cancel; 

        public Alert(string title, string message, string cancel)
        {
            Title = title;
            Message = message;
            Cancel = cancel; 
        }
    }
}
