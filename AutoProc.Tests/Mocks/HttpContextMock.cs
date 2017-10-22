using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoProc.Tests.Mocks
{
    public class HttpContextMock
    {
        private static readonly RequestDelegate NullHandler = (c) => Task.FromResult(0);
        private static IInlineConstraintResolver _inlineConstraintResolver = GetInlineConstraintResolver();

        //public static Mock<HttpContext> Create(string method, string host, string path, string querystring)
        //{
        //    if (querystring == null) querystring = "";
        //    var requestMock = new Mock<HttpRequest>();
        //    requestMock.Setup(x => x.Scheme).Returns("http");
        //    requestMock.Setup(x => x.Host).Returns(new HostString(host));
        //    requestMock.Setup(x => x.Path).Returns(new PathString(path));
        //    requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
        //    requestMock.Setup(x => x.Method).Returns(method.ToUpper());
        //    requestMock.Setup(x => x.Body).Returns(new MemoryStream());
        //    requestMock.Setup(x => x.QueryString).Returns(new QueryString("?" + querystring));


        //    var responseMock = new Mock<HttpResponse>();
        //    int statusCode = 0;
        //    responseMock.SetupProperty(x => x.StatusCode, statusCode);

        //    var contextMock = new Mock<HttpContext>();
        //    contextMock.Setup(x => x.Request).Returns(requestMock.Object);
        //    contextMock.Setup(x => x.Response).Returns(responseMock.Object);

        //    return contextMock;
        //}

        //public static Mock<HttpContext> Create(string path, string querystring = "")
        //{
        //    return Create("GET", "localhost", path, querystring);
        //}

        public static async Task<DefaultHttpContext> Create(string source = null, string schema = null, string type = null, string procedure = null)
        {
            var context = new DefaultHttpContext();

            context.Request.Path = GetRequestPath(source, schema, type, procedure);
          
            var routerContext = CreateRouteContext(context.Request.Path);
            var route = CreateRoute("api/autoproc/{source}/{schema}/{type}/{procedure}");
            await route.RouteAsync(routerContext);

            var routingFeature = new RoutingFeature();
            routingFeature.RouteData = routerContext.RouteData;
            context.Features.Set<IRoutingFeature>(routingFeature);
            return context;
        }

        public static Stream GenerateStreamFromString(string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static string GetRequestPath(string source, string schema, string type, string procedure)
        {
            return $"/api/autoproc/{source}/{schema}/{type}/{procedure}";
        }

        private static RouteContext CreateRouteContext(string requestPath, ILoggerFactory factory = null)
        {
            if (factory == null)
            {
                factory = NullLoggerFactory.Instance;
            }

            var request = new Mock<HttpRequest>(MockBehavior.Strict);
            request.SetupGet(r => r.Path).Returns(requestPath);

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context.Setup(m => m.RequestServices.GetService(typeof(ILoggerFactory)))
                .Returns(factory);
            context.SetupGet(c => c.Request).Returns(request.Object);

            return new RouteContext(context.Object);
        }

        private static Route CreateRoute(string routeName, string template, bool handleRequest = true)
        {
            return new Route(
                CreateTarget(handleRequest),
                routeName,
                template,
                defaults: null,
                constraints: null,
                dataTokens: null,
                inlineConstraintResolver: _inlineConstraintResolver);
        }

        private static Route CreateRoute(string template, bool handleRequest = true)
        {
            return new Route(CreateTarget(handleRequest), template, _inlineConstraintResolver);
        }

        private static Route CreateRoute(
            string template,
            object defaults,
            bool handleRequest = true,
            object constraints = null,
            object dataTokens = null)
        {
            return new Route(
                CreateTarget(handleRequest),
                template,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new RouteValueDictionary(dataTokens),
                _inlineConstraintResolver);
        }

        private static Route CreateRoute(IRouter target, string template)
        {
            return new Route(
                target,
                template,
                new RouteValueDictionary(),
                constraints: null,
                dataTokens: null,
                inlineConstraintResolver: _inlineConstraintResolver);
        }

        private static Route CreateRoute(
            IRouter target,
            string template,
            object defaults,
            RouteValueDictionary dataTokens = null)
        {
            return new Route(
                target,
                template,
                new RouteValueDictionary(defaults),
                constraints: null,
                dataTokens: dataTokens,
                inlineConstraintResolver: _inlineConstraintResolver);
        }

        private static IRouter CreateTarget(bool handleRequest = true)
        {
            var target = new Mock<IRouter>(MockBehavior.Strict);
            target
                .Setup(e => e.GetVirtualPath(It.IsAny<VirtualPathContext>()))
                .Returns<VirtualPathContext>(rc => null);

            target
                .Setup(e => e.RouteAsync(It.IsAny<RouteContext>()))
                .Callback<RouteContext>((c) => c.Handler = handleRequest ? NullHandler : null)
                .Returns(Task.FromResult<object>(null));

            return target.Object;
        }

        private static IInlineConstraintResolver GetInlineConstraintResolver()
        {
            var routeOptions = new Mock<IOptions<RouteOptions>>();
            routeOptions
                .SetupGet(o => o.Value)
                .Returns(new RouteOptions());

            return new DefaultInlineConstraintResolver(routeOptions.Object);
        }
    }
}
