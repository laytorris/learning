using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalService
{

    public class SQLCommandException : Exception
    {
        public Exception Exception;

        public SQLCommandException(Exception exception)
        {
            Exception = exception;
        }
    }
}