using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class RequestParser
    {
        IParameterProcessor ParameterProcessor;
        AutoProcContextOptions Options;
        public RequestParser(AutoProcContextOptions options)
        {
            ParameterProcessor = new ParameterProcessor(options);//TODO DI
            Options = options;
        }
        public AutoProcRequest GetRequest(HttpContext context)
        {
            var result = new AutoProcRequest()
            {
                Source = context.GetRouteValue("source")?.ToString().ToLower(),
                Schema = context.GetRouteValue("schema")?.ToString(),
                Type = context.GetRouteValue("type")?.ToString(),
                Procedure = context.GetRouteValue("procedure")?.ToString(),
                Method = context.Request.Method,
                Parameters = ParameterProcessor.GetRequestParameters(context)
                                    //.ToExpando() as object
            };

            return result;
        }
    }
}
