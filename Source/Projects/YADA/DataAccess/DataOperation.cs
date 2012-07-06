using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace YADA.DataAccess
{
    internal class DataOperation : IDisposable
    {
        protected SqlCommand _command;
        protected SqlConnection _conn;
        private IEnumerable<Parameter> _parameters;

        public DataOperation(string storeProcedure, IEnumerable<Parameter> parameters)
        {
            CommandText = storeProcedure;
            Parameters = parameters;
        }

        private string CommandText { get; set; }

        private IEnumerable<Parameter> Parameters
        {
            get { return _parameters ?? (_parameters = new List<Parameter>()); }
            set { _parameters = value; }
        }

        public void Dispose()
        {
            if (_conn != null && _conn.State == ConnectionState.Open) _conn.Close();

            if (_command != null) _command.Dispose();
            if (_conn != null) _conn.Dispose();
        }

        public void ExecuteNonQuery()
        {
            using (CreateConnection())
            using (CreateCommand())
            {
                AddParameters();
                OpenConnection();
                _command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public IDataReader RetrieveRecord(CommandBehavior commandBehavior)
        {
            CreateConnection();
            using (CreateCommand())
            {
                AddParameters();

                OpenConnection();

                var reader = _command.ExecuteReader(commandBehavior);

                return reader;
            }
        }

        private void AddParameters()
        {
            // Adding clear here, however I do not think I should be.  Find later why a clear is required.
            foreach(var parameter in Parameters)
            {
                if (_command.Parameters.Contains(parameter.SqlParameter.ParameterName)) _command.Parameters[parameter.SqlParameter.ParameterName] = parameter.SqlParameter;
                else _command.Parameters.Add(parameter.SqlParameter);
            }
        }

        private void CloseConnection()
        {
            _conn.Close();
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
            _conn = new SqlConnection(ConfigurationManager.ConnectionString);

            return _conn;
        }

        private void OpenConnection()
        {
            _conn.Open();
        }
    }
}