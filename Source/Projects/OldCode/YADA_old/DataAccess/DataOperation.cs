using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using YADA.Extensions;

namespace YADA.DataAccess
{
    internal class DataOperation : IDisposable
    {
        private static CommandBehavior GetCommandBehavior(Options options)
        {
            return options.IsFlagSet(Options.SingleRow) ? CommandBehavior.SingleRow : CommandBehavior.Default;
        }

        protected SqlCommand _command;
        protected SqlConnection _conn;
        private IEnumerable<Parameter> _parameters;

        public DataOperation(string commandText, IEnumerable<Parameter> parameters)
        {
            CommandText = commandText;
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

        public IDataReader RetrieveRecord(Options options)
        {
            CreateConnection();
            using (CreateCommand())
            {
                if (options.IsFlagSet(Options.StoreProcedure)) _command.CommandType = CommandType.StoredProcedure;

                AddParameters();

                OpenConnection();

                var reader = _command.ExecuteReader(GetCommandBehavior(options));

                return reader;
            }
        }

        private void AddParameters()
        {
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
            return _command = new SqlCommand(CommandText, _conn);
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