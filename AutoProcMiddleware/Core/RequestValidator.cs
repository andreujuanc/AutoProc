using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class RequestValidator
    {
        public bool ValidateRequest(AutoProcRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Procedure)) return false;
            if (string.IsNullOrWhiteSpace(request.Schema)) return false;
            if (string.IsNullOrWhiteSpace(request.Source)) return false;
            if (string.IsNullOrWhiteSpace(request.Type)) return false;
            if (string.IsNullOrWhiteSpace(request.Method)) return false;
            // Source = context.GetRouteValue("source")?.ToString().ToLower(),
            // Schema = context.GetRouteValue("schema")?.ToString(),
            // Type = context.GetRouteValue("type")?.ToString(),
            // Procedure = context.GetRouteValue("procedure")?.ToString(),
            // Method = context.Request.Method,
            // Parameters = ParameterProcessor.GetRequestParameters(context)


            return true;
        }
    }
}
