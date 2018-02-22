using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class ParameterProcessor : IParameterProcessor
    {
        AutoProcContextOptions Options;
        public ParameterProcessor(AutoProcContextOptions options)
        {
            Options = options;
        }
        
        public IDictionary<string, object> GetRequestParameters(HttpContext httpContext)
        {
            var rp = new Dictionary<string, object>();

            var serializer = new JsonSerializer();

            try
            {
                IDictionary<string, object> result = null;

                using (var sr = new StreamReader(httpContext.Request.Body))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    result = serializer.Deserialize<ExpandoObject>(jsonTextReader);
                    //jsonTextReader.
                    //result = JsonConvert.<ExpandoObject>(jsonTextReader, new ExpandoObjectConverter());
                }
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        //if (item.Key.ToLower() == "idfabrica")
                        //    continue;
                        rp.Add(item.Key, item.Value);
                    }
                }

            }
            catch
            {

            }


            foreach (var item in httpContext.Request.Query)
            {
                //    if (item.Key.ToLower() == "idfabrica" || item.Key.ToLower() == "v")
                //        continue;
                if (httpContext.User != null && item.Key.ToLower() == "includeuser" && item.Value == "true")
                {
                    rp.Add("ID_USER_LLAMADA", httpContext.User.GetUserId());
                    continue;
                }

                if (rp.ContainsKey(item.Key))
                    rp[item.Key] = item.Value[0];
                else
                    rp.Add(item.Key, item.Value[0]);
            }


            return rp;
        }
    }
}
