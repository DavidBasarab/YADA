using System;
using System.Data;
using System.Data.SqlClient;

namespace YADA.DataAccess
{
    internal class DataOperation : IDisposable
    {
        protected SqlCommand _command;
        protected SqlConnection _conn;

        public DataOperation(string storeProcedure, Parameter parameter)
        {
            CommandText = storeProcedure;
            Parameter = parameter;
        }

        public string CommandText { get; set; }
        public Parameter Parameter { get; set; }

        public void Dispose()
        {
            if (_conn.State == ConnectionState.Open) _conn.Close();

            _command.Dispose();
            _conn.Dispose();
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

        public IDataReader RetrieveRecord()
        {
            CreateConnection();
            using (CreateCommand())
            {
                _command.Parameters.Add(Parameter.SqlParameter);

                _conn.Open();

                var reader = _command.ExecuteReader(CommandBehavior.SingleRow);

                return reader;
            }
        }
    }
}