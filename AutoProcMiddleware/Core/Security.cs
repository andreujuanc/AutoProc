using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
//using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AutoProcMiddleware
{
    public static class Security
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            //claim http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier

            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
