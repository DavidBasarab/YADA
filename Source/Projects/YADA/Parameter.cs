using System.Data;
using System.Data.SqlClient;

namespace YADA
{
    public class Parameter
    {
        public static Parameter Create(string name, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            return new Parameter(name, value, parameterDirection);
        }

        private ParameterDirection _direction;

        private string _name;

        private SqlParameter _sqlParameter;

        public Parameter(string name, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            Name = name;
            Value = value;
            Direction = parameterDirection;
        }

        public ParameterDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;

                if (_sqlParameter != null) _sqlParameter.Direction = value;
            }
        }

        protected object Value { get; set; }

        public string Name
        {
            get { return _name; }
            protected set { _name = !value.StartsWith("@") ? string.Format("@{0}", value) : value; }
        }

        public SqlParameter SqlParameter
        {
            get { return _sqlParameter ?? (CreateSqlParameter()); }
        }

        public object SqlValue
        {
            get { return SqlParameter.Value; }
        }

        private SqlParameter CreateSqlParameter()
        {
            _sqlParameter = new SqlParameter(Name, Value)
                            {
                                Direction = Direction
                            };

            return _sqlParameter;
        }
    }
}