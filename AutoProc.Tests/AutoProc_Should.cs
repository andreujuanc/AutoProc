using AutoProc.Tests.Mocks;
using System;
using System.Data;
using Xunit;

namespace AutoProc.Tests
{
    public class AutoProc_Should
    {
        [Fact]
        public async void Invoke_Fail_EmptyRequest()
        {
            var context = await HttpContextMock.Create();
            var apm = CreateAutoProc();
            await apm.Invoke(context);
            Assert.Equal(400, context.Response.StatusCode);
        }

        [Fact]
        public async void Invoke_Fail_BadRequest()
        {
            var context = await HttpContextMock.Create();
            var apm = CreateAutoProc();
            await apm.Invoke(context);
            Assert.Equal(400, context.Response.StatusCode);
        }

        [Fact]
        public async void Invoke_Get_Simple()
        {
            var context = await HttpContextMock.Create("GET", "central", "dbo", "Z", "Test"); 
            var apm = CreateAutoProc();
            await apm.Invoke(context);
            Assert.Equal(200, context.Response.StatusCode);
        }

        [Fact]
        public async void Invoke_NoParameters()
        {
            var context = await HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            var apm = CreateAutoProc(new AutoProcMiddleware.Core.AutoProcContextOptions());
            await apm.Invoke(context);
            Assert.Equal(400, context.Response.StatusCode);
        }

        private AutoProcMiddleware.AutoProcMiddleware CreateAutoProc(AutoProcMiddleware.Core.AutoProcContextOptions options = null)
        {
            return new AutoProcMiddleware.AutoProcMiddleware(options ?? new AutoProcMiddleware.Core.AutoProcContextOptions()
            {
                OnNeedDbConnection = (context, aprequest) => new DbConnectionMock()
            });
        }
    }
}
