using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Yada
{
    internal class Creator<T> : BaseSetting<T> where T : class, new()
    {
        public static T Create(IDataReader reader)
        {
            return !reader.Read() ? default(T) : new Creator<T>(reader).Create();
        }

        public static IList<T> CreateList(IDataReader reader)
        {
            return new Creator<T>(reader).CreateFromList();
        }

        private Creator(IDataReader reader)
            : base(reader) {}

        private T Create()
        {
            var item = new T();

            // For loop to prevent threading issues
            for (var i = 0; i < mappingInfo.Properties.Count; i++)
            {
                var propertyInfo = mappingInfo.Properties[i];

                if (!OrdinalMap.ContainsKey(propertyInfo.Name)) continue;

                if (propertyInfo.MemberExpression.Member.DeclaringType == type) SetRootProperty(propertyInfo, item);
                else SetChildProperty(propertyInfo, item);
            }

            return item;
        }

        private IList<T> CreateFromList()
        {
            var list = new List<T>();

            while (Reader.Read())
            {
                var current = Create();

                list.Add(current);
            }

            return list;
        }

        private void SetChildProperty(PropertyMappingInfo propertyInfo, T item)
        {
            var parentProperty = type.GetProperty(((MemberExpression)propertyInfo.MemberExpression.Expression).Member.Name);
            var parentType = parentProperty.PropertyType;

            var parentValue = parentProperty.GetValue(item, null);

            var isParentNull = parentValue == null;

            if (isParentNull) parentValue = Activator.CreateInstance(parentType);

            var childProperty = parentType.GetProperty(propertyInfo.MemberExpression.Member.Name);

            SetPropertyValue(propertyInfo, childProperty, parentValue);

            if (isParentNull) parentProperty.SetValue(item, parentValue, null);
        }

        private void SetPropertyValue(PropertyMappingInfo propertyInfo, PropertyInfo property, object item)
        {
            var ordinalValue = OrdinalMap[propertyInfo.Name];

            var readerValue = Reader.GetValue(ordinalValue);

            if (readerValue == DBNull.Value) return;

            var dbType = Reader.GetFieldType(ordinalValue);
            var clrType = property.PropertyType;
            var nullableType = Nullable.GetUnderlyingType(clrType);

            try
            {
                if (clrType.BaseType == typeof(Enum) || (nullableType != null && nullableType.BaseType == typeof(Enum))) property.SetValue(item, Convert.ChangeType(readerValue, typeof(int)), null);
                else if (clrType == typeof(bool) && dbType == typeof(string)) property.SetValue(item, readerValue.ToString() == "Y", null);
                else if (clrType.IsAssignableFrom(dbType)) property.SetValue(item, readerValue, null);
                else if (nullableType != null)
                {
                    if (clrType == typeof(bool?) && dbType == typeof(string)) property.SetValue(item, readerValue.ToString() == "Y", null);
                    else property.SetValue(item, Convert.ChangeType(readerValue, nullableType), null);
                }
                else property.SetValue(item, Convert.ChangeType(readerValue, clrType), null);
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("Error mapping field {0} to value {1}", propertyInfo.Name, readerValue), ex);
            }
        }

        private void SetRootProperty(PropertyMappingInfo propertyInfo, T item)
        {
            var property = type.GetProperty(propertyInfo.MemberExpression.Member.Name);

            PropertyValueSetter<T>.SetPropertyValue(propertyInfo, property, item, Reader, OrdinalMap);
        }
    }
}