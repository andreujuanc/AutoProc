using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoProc.Tests.Mocks
{
    public class DbConnectionMock : IDbConnection
    {
        public string ConnectionString { get; set; }

        public int ConnectionTimeout => throw new NotImplementedException();

        public string Database => throw new NotImplementedException();

        public ConnectionState State { get; private set; }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.State = ConnectionState.Closed;
        }

        public IDbCommand CreateCommand()
        {
            return new DbCommandMock();
        }

        public void Dispose()
        {
            this.State = ConnectionState.Closed;
        }

        public void Open()
        {
            this.State = ConnectionState.Open;
        }
    }
}
