using System;
using System.Collections.Generic;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class AutoProcRequest
    {
        public string Source { get;  set; }
        public string Schema { get;  set; }
        public string Type { get;  set; }
        public string Procedure { get;  set; }
        public string Method { get;  set; }
        public IDictionary<string, object> Parameters { get; set; }
    }
}
