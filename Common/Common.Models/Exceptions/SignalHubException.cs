using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Exceptions
{
    public class SignalHubException : Exception
    {
        public SignalHubException()
        {
        }

        public SignalHubException(string message) : base(message)
        {
        }

        public SignalHubException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
