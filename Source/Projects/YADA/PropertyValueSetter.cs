using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Yada
{
    internal class PropertyValueSetter<T> : BaseSetting<T> where T : class, new()
    {
        public static void SetPropertyValue(PropertyMappingInfo propertyInfo, PropertyInfo property, object value, IDataReader reader, IDictionary<string, int> ordinalMap)
        {
            var valueSetter = new PropertyValueSetter<T>(propertyInfo, property, value, reader, ordinalMap);

            valueSetter.SetValue();
        }

        private Type clrType;
        private Type dbType;
        private Type nullableType;
        private int ordinalValue;
        private object readerValue;

        public PropertyValueSetter(PropertyMappingInfo propertyMappingInfo, PropertyInfo propertyInfo, object value, IDataReader reader, IDictionary<string, int> ordinalMap)
            : base(reader, ordinalMap)
        {
            PropertyMappingInfo = propertyMappingInfo;
            PropertyInfo = propertyInfo;
            Value = value;
        }

        private PropertyMappingInfo PropertyMappingInfo { get; set; }
        private PropertyInfo PropertyInfo { get; set; }
        private object Value { get; set; }

        private bool IsNullableType
        {
            get { return nullableType != null; }
        }

        private bool IsAssignableFromDbType
        {
            get { return clrType.IsAssignableFrom(dbType); }
        }

        private bool IsStringToBool
        {
            get { return clrType == typeof(bool) && dbType == typeof(string); }
        }

        private bool IsEnum
        {
            get { return clrType.BaseType == typeof(Enum) || (nullableType != null && nullableType.BaseType == typeof(Enum)); }
        }

        private void AttemptSetValue()
        {
            if (IsEnum) SetEnum();
            else if (IsStringToBool) ConvertStringToBool();
            else if (IsAssignableFromDbType) SetAssignableValue();
            else if (IsNullableType) SetNullableType();
            else BasicSetValue();
        }

        private void BasicSetValue()
        {
            PropertyInfo.SetValue(Value, Convert.ChangeType(readerValue, clrType), null);
        }

        private void ConvertStringToBool()
        {
            PropertyInfo.SetValue(Value, readerValue.ToString() == "Y", null);
        }

        private void SetAssignableValue()
        {
            PropertyInfo.SetValue(Value, readerValue, null);
        }

        private void SetEnum()
        {
            PropertyInfo.SetValue(Value, Convert.ChangeType(readerValue, typeof(int)), null);
        }

        private void SetNullableType()
        {
            if (clrType == typeof(bool?) && dbType == typeof(string)) PropertyInfo.SetValue(Value, readerValue.ToString() == "Y", null);
            else PropertyInfo.SetValue(Value, Convert.ChangeType(readerValue, nullableType), null);
        }

        private void SetValue()
        {
            ordinalValue = OrdinalMap[PropertyMappingInfo.Name];

            readerValue = Reader.GetValue(ordinalValue);

            if (readerValue == DBNull.Value) return;

            GetTypes();

            try
            {
                AttemptSetValue();
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("Error mapping field {0} to value {1}", PropertyMappingInfo.Name, readerValue), ex);
            }
        }

        private void GetTypes()
        {
            dbType = Reader.GetFieldType(ordinalValue);
            clrType = PropertyInfo.PropertyType;
            nullableType = Nullable.GetUnderlyingType(clrType);
        }
    }
}