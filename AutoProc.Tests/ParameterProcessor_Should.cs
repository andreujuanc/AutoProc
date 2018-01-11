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
                " );
            var parameters = ParameterProcessor.GetRequestParameters(context);
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
            var parameters = ParameterProcessor.GetRequestParameters(context);
            Assert.Equal(2, parameters.Count);
            Assert.Equal("1", parameters["somefilter"]); //WE CANT CHECK TYPES FROM QUERYSTRING
            Assert.Equal("testing", parameters["stringFilter"]);
        }

    }
}
