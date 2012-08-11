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
        private PropertyReflectionManager _reflectionManager;

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

        private PropertyReflectionManager ReflectionManager
        {
            get { return _reflectionManager ?? (_reflectionManager = new PropertyReflectionManager()); }
        }

        public TEntity GetRecord<TEntity>(string commandText, IEnumerable<Parameter> parameters = null, Options options = Options.None) where TEntity : new()
        {
            TEntity newObject;

            using (var reader = Reader.RetrieveRecord(commandText, parameters, Options.SingleRow | options))
            {
                IDictionary<string, int> columnsOrdinalRef = new Dictionary<string, int>();

                reader.Read();

                PopulateOrdinalReference(columnsOrdinalRef, reader);

                newObject = CreateFromDataRecord<TEntity>(reader, columnsOrdinalRef);

                reader.Close();
            }

            return newObject;
        }

        public IList<TEntity> GetRecords<TEntity>(string commandText, IEnumerable<Parameter> parameters = null, Options options = Options.None) where TEntity : new()
        {
            var records = new List<TEntity>();

            using (var reader = Reader.RetrieveRecord(commandText, parameters, options))
            {
                IDictionary<string, int> columnsOrdinalRef = new Dictionary<string, int>();

                while (reader.Read())
                {
                    PopulateOrdinalReference(columnsOrdinalRef, reader);

                    records.Add(CreateFromDataRecord<TEntity>(reader, columnsOrdinalRef));
                }

                reader.Close();
            }

            return records;
        }

        private static void PopulateOrdinalReference(IDictionary<string, int> columnsOrdinalRef, IDataReader reader)
        {
            if (columnsOrdinalRef.Count == 0)
            {
                var fieldCount = reader.FieldCount;

                for (var i = 0; i < fieldCount; i++) columnsOrdinalRef.Add(reader.GetName(i), i);
            }
        }

        public void InsertRow(string procedureName, IEnumerable<Parameter> parameters)
        {
            var dataOperation = new DataOperation(procedureName, parameters);

            dataOperation.ExecuteNonQuery();
        }

        private TEntity CreateFromDataRecord<TEntity>(IDataRecord reader, IDictionary<string, int> columnsOrdinalRef) where TEntity : new()
        {
            var newObject = new TEntity();

            var helper = ReflectionManager.GetFromCache<TEntity>();

            foreach(var property in helper.Properties)
            {
                var ordinalValue = columnsOrdinalRef[property.PropertyName];
                
                var value = reader[ordinalValue];

                if (value is DBNull) value = null;

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