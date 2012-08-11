using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using YADA.DataAccess;
using YADA.PropertyReflection;

namespace YADA
{
    public class Database
    {
        public static Database Instance
        {
            get { return Nested.Instance; }
        }

        private IDictionary<Type, IEnumerable<PropertyInfo>> _entitiesProperties;

        private Reader _reader;

        public Database() {}

        internal Database(Reader reader)
        {
            _reader = reader;
        }

        private Reader Reader
        {
            get { return _reader ?? (_reader = new YadaReader()); }
        }

        public IDictionary<Type, IEnumerable<PropertyInfo>> EntitiesProperties
        {
            get { return _entitiesProperties ?? (_entitiesProperties = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>()); }
        }

        public TEntity GetRecord<TEntity>(string commandText, IEnumerable<Parameter> parameters = null, Options options = Options.None) where TEntity : new()
        {
            TEntity newObject;

            using (var reader = Reader.RetrieveRecord(commandText, parameters, Options.SingleRow | options))
            {
                reader.Read();

                newObject = CreateFromReader<TEntity>(reader);

                reader.Close();
            }

            return newObject;
        }

        public IList<TEntity> GetRecords<TEntity>(string commandText, IEnumerable<Parameter> parameters = null, Options options = Options.None) where TEntity : new()
        {
            var records = new List<TEntity>();

            using (var reader = Reader.RetrieveRecord(commandText, parameters, options))
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

        private PropertyReflectionManager _reflectionManager;
        private PropertyReflectionManager ReflectionManager
        {
            get { return _reflectionManager ?? (_reflectionManager = new PropertyReflectionManager()) ; }
        }

        private TEntity CreateFromReader<TEntity>(IDataRecord reader) where TEntity : new()
        {
            var newObject = new TEntity();

            var helper = ReflectionManager.GetFromCache<TEntity>();

            foreach(var property in helper.Properties)
            {
                var value = reader[property.PropertyName];

                if (value is DBNull)
                {
                    value = null;
                }

                property.SetProperty(newObject, value);
            }

            return newObject;
        }

        private class Nested
        {
            internal static readonly Database Instance;

            static Nested()
            {
                Instance = new Database();
            }
        }
    }
}