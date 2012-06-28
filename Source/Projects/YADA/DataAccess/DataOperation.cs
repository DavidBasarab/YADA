using System;
using System.Data;
using System.Data.SqlClient;

namespace YADA.DataAccess
{
    internal class DataOperation : IDisposable
    {
        public static IDataReader RetrieveRecord(string storeProcedure, Parameter parameter)
        {
            return new DataOperation(storeProcedure, parameter).RetrieveRecord();
        }

        protected SqlCommand _command;
        protected SqlConnection _conn;

        private DataOperation(string storeProcedure, Parameter parameter)
        {
            CommandText = storeProcedure;
            Parameter = parameter;
        }

        public string CommandText { get; set; }
        public Parameter Parameter { get; set; }

        public void Dispose()
        {
            if (_conn.State == ConnectionState.Open) _conn.Close();
        }

        private SqlCommand CreateCommand()
        {
            return _command = new SqlCommand(CommandText, _conn)
                              {
                                  CommandType = CommandType.StoredProcedure
                              };
        }

        private SqlConnection CreateConnection()
        {
            return _conn = new SqlConnection(ConfigurationManager.ConnectionString);
        }

        private IDataReader RetrieveRecord()
        {
            CreateConnection();
            using (CreateCommand())
            {
                _command.Parameters.Add(Parameter.SqlParameter);

                _conn.Open();

                var reader = _command.ExecuteReader(CommandBehavior.SingleRow);

                //_conn.Close();

                return reader;
            }
        }
    }
}