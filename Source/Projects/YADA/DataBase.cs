using System.Collections.Generic;
using System.Data;
using YADA.DataAccess;

namespace YADA
{
    public class Database<TEntity> where TEntity : new()
    {
        public static TEntity GetRecord(string procedureName, IEnumerable<Parameter> parameters = null)
        {
            TEntity newObject;

            using (var reader = YadaReader.RetrieveRecord(procedureName, parameters, CommandBehavior.SingleRow))
            {
                reader.Read();

                newObject = CreateFromReader(reader);

                reader.Close();
            }

            return newObject;
        }

        public static IList<TEntity> GetRecords(string procedure, IEnumerable<Parameter> parameters = null)
        {
            var records = new List<TEntity>();

            using (var reader = YadaReader.RetrieveRecord(procedure, parameters))
            {
                while (reader.Read()) records.Add(CreateFromReader(reader));

                reader.Close();
            }

            return records;
        }

        private static TEntity CreateFromReader(IDataRecord reader)
        {
            var newObject = new TEntity();

            var properties = typeof(TEntity).GetProperties();

            foreach(var propertyInfo in properties)
            {
                var value = reader[propertyInfo.Name];

                propertyInfo.SetValue(newObject, value, null);
            }

            return newObject;
        }
    }

    public class Database
    {
        public static void InsertRow(string procedureName, IEnumerable<Parameter> parameters)
        {
            var dataOperation = new DataOperation(procedureName, parameters);

            dataOperation.ExecuteNonQuery();
        }
    }
}