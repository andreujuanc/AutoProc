using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AutoProcMiddleware.Core
{
    public class ContextOptions
    {
        public Func<DbConnection> OnNeedDbConnection;
    }
}
