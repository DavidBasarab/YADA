using System.Collections.Generic;
using System.Data;
using YADA.DataAccess;

namespace YADA
{
    public class Database
    {
        public Database() {}

        internal Database(Reader reader)
        {
            Reader = reader;
        }

        private Reader Reader { get; set; }

        public TEntity GetRecord<TEntity>(string procedureName, IEnumerable<Parameter> parameters = null) where TEntity : new()
        {
            TEntity newObject;

            using (var reader = Reader.RetrieveRecord(procedureName, parameters, CommandBehavior.SingleRow))
            {
                reader.Read();

                newObject = CreateFromReader<TEntity>(reader);

                reader.Close();
            }

            return newObject;
        }

        public IList<TEntity> GetRecords<TEntity>(string procedure, IEnumerable<Parameter> parameters = null) where TEntity : new()
        {
            var records = new List<TEntity>();

            using (var reader = ADOReader.RetrieveRecord(procedure, parameters))
            {
                while (reader.Read()) records.Add(CreateFromReader<TEntity>(reader));

                reader.Close();
            }

            return records;
        }

        public void InsertRow(string procedureName, IEnumerable<Parameter> parameters)
        {
            var dataOperation = new DataOperation(procedureName, parameters);

            dataOperation.ExecuteNonQuery();
        }

        private TEntity CreateFromReader<TEntity>(IDataRecord reader) where TEntity : new()
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
}