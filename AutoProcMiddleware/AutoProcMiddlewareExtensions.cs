using AutoProcMiddleware.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoProcMiddleware
{
    public static class AutoProcMiddlewareExtensions
    {
        /// <summary>
        /// Adds AutoProc to the pipeline 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="optionBuilder">A callback to configure path, templates, and connection options</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoProc(this IApplicationBuilder app, Action<AutoProcContextOptions> optionBuilder = null)
        {
            AutoProcContextOptions options = null;
            var handler = new RouteHandler(async context =>
            {
                //if (!context.User.Identity.IsAuthenticated)
                //    throw new UnauthorizedAccessException();

                await new AutoProcMiddleware(options).Invoke(context);
            });

            if (options == null)
            {
                options = new AutoProcContextOptions();
                optionBuilder?.Invoke(options);
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrWhiteSpace(options.Path))
            {
                throw new ArgumentNullException(nameof(options.Path));
            }

            if (options.OnNeedDbConnection == null)
            {
                options.OnNeedDbConnection = (context, aprequest) => { return context.RequestServices.GetService(typeof(IDbConnection)) as IDbConnection; };
            }

            var routeBuilder = new RouteBuilder(app, handler);
            routeBuilder.MapRoute("Main", options.Path + "{source}/{schema}/{type}/{procedure}");
            var routes = routeBuilder.Build();
            return app.UseRouter(routes);
            //return app.MapRoute("", x => new AutoProcMiddleware(x));
        }
    }
}
