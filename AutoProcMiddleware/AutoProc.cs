using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
//using Newtonsoft.Json;
using System.Collections;
using System.Data;
using AutoProcMiddleware.Core;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.Authorization;
//using static Dapper.SqlMapper;

namespace AutoProcMiddleware
{
    public class AutoProcMiddleware

    {
        private AutoProcContextOptions _options = null;
        
        public AutoProcMiddleware(AutoProcContextOptions options)
        {
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            httpContext.Response.ContentType = "text/html charset=utf-8";
            httpContext.Response.StatusCode = 400;
            var apr = new RequestParser()
               .GetRequest(httpContext);
            if(!new RequestValidator().ValidateRequest(apr)) return;
            var result = await ExecuteAsync(httpContext, apr);

            if (result == null)
            {
                await httpContext.Response.WriteAsync($"Invalid call for: {apr.Procedure} without any result");
                return;
            }

            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json charset=utf-8";
            if (_options.BypassORM && result.Count() == 1)
            {
                var row = result.First();

                var dataItem = ((System.Collections.Generic.IDictionary<string, object>)row);

                var dataMember = dataItem.Keys.Where(x => x.StartsWith("JSON")).FirstOrDefault();
                if (dataMember != null)
                {
                    var jsonResult = dataItem[dataMember] as string;
                    await httpContext.Response.WriteAsync(jsonResult);
                }
                else
                {
                    httpContext.Response.StatusCode = 500;
                    await httpContext.Response.WriteAsync("Invalid JSON Result.");
                }

            }
            else
            {
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result));
            }

        }

        internal string GetProcedureName(AutoProcRequest request)
        {
            return $"{request.Schema}.P_{request.Type}_{request.Procedure}";
        }

        internal async Task<IEnumerable<dynamic>> ExecuteAsync(HttpContext httpContext, AutoProcRequest request)
        {
            var connection = GetOpenDbContext(httpContext, request);

            if (connection == null || connection.State != ConnectionState.Open)
            {
                await httpContext.Response.WriteAsync("Could not establish connection");
                return null;
            }

            //IEnumerable<dynamic> result;
            try
            {
                var ProcName = GetProcedureName(request);
                //result = await dbcon.QueryAsync(ProcName, parameters, null, null, System.Data.CommandType.StoredProcedure);
                var result = connection.Query(ProcName, request.Parameters, null, true, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                await httpContext.Response.WriteAsync($"Invalid call for: {request.Procedure}. Error: {ex.Message}");
            }
            finally
            {
                connection?.Close();
                connection?.Dispose();
            }
            return null;
        }

        private IDbConnection GetOpenDbContext(HttpContext context, AutoProcRequest aprequest)
        {
            var cnn =  _options?.OnNeedDbConnection?.Invoke(context, aprequest);
            if(cnn != null && cnn.State !=  ConnectionState.Open)
                cnn.Open();
            return cnn;
        }


        //internal async Task<GridReader> ExecuteMultipleAsync(HttpContext httpContext, string source, string schema, string type, string procedure, string method, object parameters)
        //{

        //    //httpContext.Response.
        //    var connection = (IContextConnection)httpContext.RequestServices.GetService(typeof(IContextConnection));
        //    IDbConnection dbcon = null;
        //    if (source == "central")
        //    {
        //        if (connection == null || connection.CentralConnection == null)
        //        {
        //            await httpContext.Response.WriteAsync("Could not establish connection with central");
        //            return null;
        //        }
        //        else
        //            dbcon = connection.CentralConnection;
        //    }
        //    else if (source == "client")
        //    {
        //        if (connection == null || connection.ClientConnection == null)
        //        {
        //            await httpContext.Response.WriteAsync("Could not establish connection with client");
        //            return null;
        //        }
        //        else
        //            dbcon = connection.ClientConnection;
        //    }
        //    else
        //    {
        //        await httpContext.Response.WriteAsync("Invalid source call");
        //        return null;
        //    }

        //    if (dbcon == null)
        //    {
        //        await httpContext.Response.WriteAsync("Could not establish connection");
        //        return null;
        //    }

        //    //IEnumerable<dynamic> result;
        //    try
        //    {
        //        var ProcName = $"{schema}.P_{type}_{procedure}";
        //        //result = await dbcon.QueryAsync(ProcName, parameters, null, null, System.Data.CommandType.StoredProcedure);
        //        var result = dbcon.QueryMultiple(ProcName, parameters, null, null, System.Data.CommandType.StoredProcedure);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        await httpContext.Response.WriteAsync($"Invalid call for: {procedure}. Error: {ex.Message}");
        //    }
        //    return null;
        //}
    }

  
}
