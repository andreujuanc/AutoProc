using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoProc.Tests.Mocks
{
    public class DbDataReaderMock : IDataReader
    {
        private List<object> Data;
        private int CurrentRowIndex = -1;
        private object CurrentRow => CurrentRowIndex >= 0 && CurrentRowIndex < Data.Count ? Data[CurrentRowIndex] : null;
        public DbDataReaderMock(List<object> data) => Data = data ?? throw new ArgumentNullException("data");

        private bool _isOpen = false;
        public object this[int i] => throw new NotImplementedException();

        public object this[string name] => throw new NotImplementedException();

        public int Depth => throw new NotImplementedException();

        public bool IsClosed => !_isOpen;

        public int RecordsAffected => throw new NotImplementedException();

        public int FieldCount => Data[0].GetType().GetProperties().Length;

        public void Close()
        {
            _isOpen = false;
        }

        public void Dispose()
        {
            _isOpen = false;
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            return Data[0].GetType().GetProperties()[i].PropertyType;
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            return Data[0].GetType().GetProperties()[i].Name;
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            return CurrentRow.GetType().GetProperties()[i].GetValue(CurrentRow);
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            CurrentRowIndex++;
            return CurrentRow != null;
        }
    }
}
