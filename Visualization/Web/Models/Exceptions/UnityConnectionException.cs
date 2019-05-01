using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Exceptions
{
    public class UnityConnectionException : Exception
    {
        public UnityConnectionException() : base()
        {
        }

        public UnityConnectionException(string msg) : base(msg)
        {
        }

        public UnityConnectionException(string msg, Exception ex) : base(msg, ex)
        {
        }
    }
}
