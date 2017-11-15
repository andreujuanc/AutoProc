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

    }
}
