using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class AutoProcContextOptions
    {

        /// <summary>
        /// Sets the connection for each execution. This allows multiple databases to be targeted based on custom conditions.
        /// </summary>
        public Func<HttpContext, AutoProcRequest, IDbConnection> OnNeedDbConnection;

        /// <summary>
        /// Allows per-request customization.
        /// Return false to unauthorize the execution.
        /// </summary>
        public Func<HttpContext, AutoProcRequest, bool> OnPreExecute;

        /// <summary>
        /// Where do you want AutoProc to live at. Default is api/autoproc/..
        /// </summary>
        public string Path { get; set; } = "api/autoproc/";

        /// <summary>
        /// If the database returns a valid json, autoproc will just write that to the response.
        /// </summary>
        public bool BypassORM { get; set; } = false;

        /// <summary>
        ///If in the request parameters includeUser = true then AutoProc will include the following parameter automatically with the authenticated user id
        /// </summary>
        public string DBUserIdParameterName { get; set; }  = "UserId";
    }
}
