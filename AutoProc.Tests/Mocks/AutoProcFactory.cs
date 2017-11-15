using System;
using System.Collections.Generic;
using System.Text;

namespace AutoProc.Tests.Mocks
{
    public class AutoProcFactory
    {
        internal static AutoProcMiddleware.AutoProcMiddleware CreateAutoProc(AutoProcMiddleware.Core.AutoProcContextOptions options = null)
        {
            return new AutoProcMiddleware.AutoProcMiddleware(options ?? new AutoProcMiddleware.Core.AutoProcContextOptions()
            {
                OnNeedDbConnection = (context, aprequest) => new DbConnectionMock()
            });
        }
    }
}
