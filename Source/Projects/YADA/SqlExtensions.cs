using System.Data.SqlClient;

namespace Yada
{
    public static class SqlExtensions
    {
        public static int ExecuteNonQuery<T>(this SqlCommand command, T value) where T : class, new()
        {
            AddParameters(command, value);

            return command.ExecuteNonQuery();
        }

        public static object ExecuteScalar<T>(this SqlCommand command, T value) where T : class, new()
        {
            AddParameters(command, value);

            return command.ExecuteScalar();
        }

        private static void AddParameters<T>(SqlCommand command, T value) where T : class, new()
        {
            var key = new ProcedureMap(command.CommandText, typeof(T));

            var mappingInfo = Mapper.InputProcedureMappingInfo[key];

            if (mappingInfo == null) throw new InvalidMapException(string.Format("Mapping missing for type {0}", typeof(T)));

            foreach(var propertyMappingInfo in mappingInfo.Properties)
            {
                var property = key.Type.GetProperty(propertyMappingInfo.MemberExpression.Member.Name);

                command.Parameters.Add(new SqlParameter(propertyMappingInfo.Name, property.GetValue(value)));
            }
        }
    }
}