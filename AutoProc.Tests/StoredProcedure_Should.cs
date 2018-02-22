using AutoProc.Tests.Mocks;
using AutoProcMiddleware.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Reflection;

namespace AutoProc.Tests
{
    public class StoredProcedure_Should
    {
        [Fact]
        public async void NotBeEmpty()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var apr = new RequestParser(options).GetRequest(context);
            var apm = AutoProcFactory.CreateAutoProc(new AutoProcMiddleware.Core.AutoProcContextOptions());
            var aptype = apm.GetType();
            var method = aptype.GetMethod("GetProcedureName", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = method.Invoke(apm, new object[] { apr } );
            Assert.Equal("dbo.P_Z_Test", result);
        }
        
    }
}
