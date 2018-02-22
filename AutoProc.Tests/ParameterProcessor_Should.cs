using AutoProcMiddleware.Core;
using Xunit;

namespace AutoProc.Tests
{
    public class ParameterProcessor_Should
    {


        [Fact]
        public async void ParseParametersFromJsonBody()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            context.Request.Body = Mocks.HttpContextMock.GenerateStreamFromString(
                @"
                    {
                        ""somefilter"": 1,
                        ""stringFilter"":""testing""
                    }
                ");
            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var parameters = new ParameterProcessor(options).GetRequestParameters(context);
            Assert.Equal(1L, parameters["somefilter"]);
            Assert.Equal("testing", parameters["stringFilter"]);
        }

        [Fact]
        public async void ParseParametersFromQueryString()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            context.Request.QueryString = new Microsoft.AspNetCore.Http.QueryString("?somefilter=1&stringFilter=testing");
            // context.Request.Body = Mocks.HttpContextMock.GenerateStreamFromString(
            //     @"
            //         {
            //             """": 1,
            //             ""stringFilter"":""testing""
            //         }
            //     " );
            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var parameters = new ParameterProcessor(options).GetRequestParameters(context);
            Assert.Equal(2, parameters.Count);
            Assert.Equal("1", parameters["somefilter"]); //WE CANT CHECK TYPES FROM QUERYSTRING
            Assert.Equal("testing", parameters["stringFilter"]);
        }

        [Fact]
        public async void NotInclude_includeUser_toDBParameters()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            context.Request.QueryString = new Microsoft.AspNetCore.Http.QueryString("?somefilter=1&stringFilter=testing&includeUser=true");

            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var parameters = new ParameterProcessor(options).GetRequestParameters(context);
            Assert.False(parameters.ContainsKey("includeUser"));
        }

        [Fact]
        public async void NotIncludeUserIdWhenNotAuthenticated()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            context.Request.QueryString = new Microsoft.AspNetCore.Http.QueryString("?somefilter=1&stringFilter=testing&includeUser=true");
            // context.Request.Body = Mocks.HttpContextMock.GenerateStreamFromString(
            //     @"
            //         {
            //             """": 1,
            //             ""stringFilter"":""testing""
            //         }
            //     " );
            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var parameters = new ParameterProcessor(options).GetRequestParameters(context);
            Assert.Equal(2, parameters.Count);
            Assert.False(parameters.ContainsKey(options.DBUserIdParameterName));
        }

        [Fact]
        public async void IncludeUserIdWhenAuthenticated()
        {
            var context = await Mocks.HttpContextMock.Create("GET", "central", "dbo", "Z", "Test");
            var UserId = "8503";
            context.Request.QueryString = new Microsoft.AspNetCore.Http.QueryString("?somefilter=1&stringFilter=testing&includeUser=true");
            context.User = new System.Security.Claims.ClaimsPrincipal(
                        new System.Security.Claims.ClaimsIdentity(
                                new System.Security.Claims.Claim[] 
                                {
                                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Administrator"),
                                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, UserId)
                                }, 
                                "Basic"
                            )
                        );
            var options = new AutoProcMiddleware.Core.AutoProcContextOptions();
            var parameters = new ParameterProcessor(options).GetRequestParameters(context);
            Assert.Equal(3, parameters.Count);
            Assert.True(parameters.ContainsKey(options.DBUserIdParameterName));
            Assert.Equal(UserId, parameters[options.DBUserIdParameterName]);
        }

    }
}
