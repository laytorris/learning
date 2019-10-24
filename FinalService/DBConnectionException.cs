using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalService
{
    public class DBConnectionException:Exception
    {
        public Exception Exception;

        public DBConnectionException(Exception exception)
        {
            Exception = exception;
        }
    }
}