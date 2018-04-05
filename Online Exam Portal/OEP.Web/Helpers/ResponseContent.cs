using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEP.Web.Helpers
{
    public class ResponseContent<T>
    {
        public string Status { get; set; }

        public T Result { get; set; }

        public string Message { get; set; }

    }
}