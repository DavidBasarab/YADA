using YADA.DataAccess;

namespace YADA
{
    public class Reader<TEntity> where TEntity : new()
    {
        public static TEntity GetRecord(string procedureName, int keyID)
        {
            var parameter = new Parameter("SmallDataID", keyID);

            var reader = DataOperation.RetrieveRecord(procedureName, parameter);

            reader.Read();

            var properties = typeof(TEntity).GetProperties();

            var newObject = new TEntity();

            foreach(var propertyInfo in properties)
            {
                var value = reader[propertyInfo.Name];

                propertyInfo.SetValue(newObject, value, null);
            }

            reader.Close();

            return newObject;
        }
    }
}