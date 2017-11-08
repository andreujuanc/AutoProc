using AutoProcMiddleware.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace AutoProc.Tests
{
    public class RequestValidator_Should
    {
        [Fact]
        public void PreventEmptyRequest()
        {
            var apr = new AutoProcRequest();
            var validator = new RequestValidator();
            Assert.False(validator.ValidateRequest(apr));
        }

        [Fact]
        public void PreventIncompleteRequest()
        {
            var apr = new AutoProcRequest();            
            var validator = new RequestValidator();
            var properties = apr.GetType().GetProperties().Where(x => x.PropertyType == typeof(string));

            foreach (var prop in properties)
            {
                foreach (var prop2 in properties)
                {
                    prop2.SetValue(apr, null);
                }
                prop.SetValue(apr, "VALUE");
                Assert.False(validator.ValidateRequest(apr));
            }
        }
    }
}
