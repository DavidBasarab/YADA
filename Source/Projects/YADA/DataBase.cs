using YADA.DataAccess;

namespace YADA
{
    public class Database<TEntity> where TEntity : new()
    {
        public static TEntity GetRecord(string procedureName, Parameter parameter)
        {
            var newObject = new TEntity();

            using (var reader = YadaReader.RetrieveRecord(procedureName, parameter))
            {
                reader.Read();

                var properties = typeof(TEntity).GetProperties();

                foreach(var propertyInfo in properties)
                {
                    var value = reader[propertyInfo.Name];

                    propertyInfo.SetValue(newObject, value, null);
                }

                reader.Close();
            }

            return newObject;
        }
    }
}