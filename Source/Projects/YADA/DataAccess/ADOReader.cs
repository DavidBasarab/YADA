using System;
using System.Collections.Generic;
using System.Data;

namespace YADA.DataAccess
{
    internal interface YADAReader
    {
        IDataReader RetrieveRecord(string storeProcedure, IEnumerable<Parameter> parameters, CommandBehavior commandBehavior = CommandBehavior.Default);
    }

    internal class ADOReader : IDataReader
    {
        public static IDataReader RetrieveRecord(string storeProcedure, IEnumerable<Parameter> parameters, CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            return new ADOReader(storeProcedure, parameters).GetReader(commandBehavior);
        }

        private ADOReader(string storeProcedure, IEnumerable<Parameter> parameters)
        {
            DataOperation = new DataOperation(storeProcedure, parameters);
        }

        public ADOReader(IDataReader reader)
        {
            Reader = reader;
        }

        private DataOperation DataOperation { get; set; }

        private IDataReader Reader { get; set; }

        public void Dispose()
        {
            Reader.Dispose();
            DataOperation.Dispose();

            Reader = null;
            DataOperation = null;
        }

        public string GetName(int i)
        {
            return Reader.GetName(i);
        }

        public string GetDataTypeName(int i)
        {
            return Reader.GetDataTypeName(i);
        }

        public Type GetFieldType(int i)
        {
            return Reader.GetFieldType(i);
        }

        public object GetValue(int i)
        {
            return Reader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return Reader.GetValues(values);
        }

        public int GetOrdinal(string name)
        {
            return Reader.GetOrdinal(name);
        }

        public bool GetBoolean(int i)
        {
            return Reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return Reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return Reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public Guid GetGuid(int i)
        {
            return Reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return Reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return Reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return Reader.GetInt64(i);
        }

        public float GetFloat(int i)
        {
            return Reader.GetFloat(i);
        }

        public double GetDouble(int i)
        {
            return Reader.GetDouble(i);
        }

        public string GetString(int i)
        {
            return Reader.GetString(i);
        }

        public decimal GetDecimal(int i)
        {
            return Reader.GetDecimal(i);
        }

        public DateTime GetDateTime(int i)
        {
            return Reader.GetDateTime(i);
        }

        public IDataReader GetData(int i)
        {
            return Reader.GetData(i);
        }

        public bool IsDBNull(int i)
        {
            return Reader.IsDBNull(i);
        }

        public int FieldCount
        {
            get { return Reader.FieldCount; }
        }

        object IDataRecord.this[int i]
        {
            get { return Reader[i]; }
        }

        object IDataRecord.this[string name]
        {
            get { return Reader[name]; }
        }

        public void Close()
        {
            Reader.Close();
            DataOperation.Dispose();
        }

        public DataTable GetSchemaTable()
        {
            return Reader.GetSchemaTable();
        }

        public bool NextResult()
        {
            return Reader.NextResult();
        }

        public bool Read()
        {
            return Reader.Read();
        }

        public int Depth
        {
            get { return Reader.Depth; }
        }

        public bool IsClosed
        {
            get { return Reader.IsClosed; }
        }

        public int RecordsAffected
        {
            get { return Reader.RecordsAffected; }
        }

        private ADOReader GetReader(CommandBehavior commandBehavior)
        {
            Reader = DataOperation.RetrieveRecord(commandBehavior);

            return this;
        }
    }
}