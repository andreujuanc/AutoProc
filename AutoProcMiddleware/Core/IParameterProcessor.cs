using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public interface IParameterProcessor
    {
    
        IDictionary<string, object> GetRequestParameters(HttpContext httpContext);
    }
}
