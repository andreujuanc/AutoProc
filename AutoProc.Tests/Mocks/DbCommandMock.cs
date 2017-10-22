using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoProc.Tests.Mocks
{
    public class DbCommandMock : IDbCommand
    {
        public string CommandText { get ; set; }
        public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CommandType CommandType { get; set; }
        public IDbConnection Connection { get ; set ; }

        public IDataParameterCollection Parameters => throw new NotImplementedException();

        public IDbTransaction Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            var data = new List<object>();
            data.Add(new { ID = 1, Text = "Hello" });
            data.Add(new { ID = 2, Text = "World" });
            return new DbDataReaderMock(data);
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
