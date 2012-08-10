using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using YADA.DataAccess;

namespace YADA
{
    [Flags]
    public enum Options
    {
        None = 0,
        SingleRow = 1,
        StoreProcedure = 2
    }

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

        private TEntity CreateFromReader<TEntity>(IDataRecord reader) where TEntity : new()
        {
            var newObject = new TEntity();

            var properties = GetProperties<TEntity>();

            foreach(var propertyInfo in properties)
            {
                var temp = propertyInfo.GetSetMethod();

                var value = reader[propertyInfo.Name];

                if (value is DBNull) value = null;

                //propertyInfo.SetValue(newObject, value, null);

                var currentType = propertyInfo.PropertyType;

                if (currentType == typeof(object))
                {
                    var setForProperty = (Action<TEntity, object>)Delegate.CreateDelegate(typeof(Action<TEntity, object>), null, temp);

                    setForProperty(newObject, value);

                    continue;
                }

                if (currentType == typeof(int))
                {
                    var setForProperty = (Action<TEntity, int>)Delegate.CreateDelegate(typeof(Action<TEntity, int>), null, temp);

                    setForProperty(newObject, (int)value);

                    continue;
                }
                if (currentType == typeof(string))
                {
                    var setForProperty = (Action<TEntity, string>)Delegate.CreateDelegate(typeof(Action<TEntity, string>), null, temp);

                    setForProperty(newObject, (string)value);

                    continue;
                }
                if (currentType == typeof(DateTime))
                {
                    var setForProperty = (Action<TEntity, DateTime>)Delegate.CreateDelegate(typeof(Action<TEntity, DateTime>), null, temp);

                    setForProperty(newObject, (DateTime)value);

                    continue;
                }
                if (currentType == typeof(Guid))
                {
                    var setForProperty = (Action<TEntity, Guid>)Delegate.CreateDelegate(typeof(Action<TEntity, Guid>), null, temp);

                    setForProperty(newObject, (Guid)value);

                    continue;
                }
                if (currentType == typeof(short))
                {
                    var setForProperty = (Action<TEntity, short>)Delegate.CreateDelegate(typeof(Action<TEntity, short>), null, temp);

                    setForProperty(newObject, (short)value);

                    continue;
                }
                
                propertyInfo.SetValue(newObject, value, null);
            }

            return newObject;
        }

        private IEnumerable<PropertyInfo> GetProperties<TEntity>() where TEntity : new()
        {
            IEnumerable<PropertyInfo> properties;
            var entityType = typeof(TEntity);

            var foundType = EntitiesProperties.TryGetValue(entityType, out properties);

            if (!foundType)
            {
                properties = entityType.GetProperties();

                EntitiesProperties.Add(entityType, properties);
            }

            return properties;
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