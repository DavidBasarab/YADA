using System;
using System.Reflection;

namespace YADA.PropertyReflection
{
    internal class PropertySetting<T>
    {
        public PropertySetting(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;

            PropertyName = PropertyInfo.Name;

            CreateSetMethod();
        }

        private Type PropertyType { get; set; }

        private Action<T, object> ObjectSet { get; set; }
        private Action<T, int> IntSet { get; set; }
        private Action<T, string> StringSet { get; set; }
        private Action<T, DateTime> DateTimeSet { get; set; }
        private Action<T, Guid> GuidSet { get; set; }
        private Action<T, short> ShortSet { get; set; }
        private Action<T, byte> ByteSet { get; set; }

        private PropertyInfo PropertyInfo { get; set; }

        public string PropertyName { get; set; }

        public void SetProperty(T newObject, object value)
        {
            if (PropertyType == typeof(object))
            {
                ObjectSet(newObject, value);

                return;
            }

            if (PropertyType == typeof(int))
            {
                IntSet(newObject, (int)value);

                return;
            }

            if (PropertyType == typeof(string))
            {
                StringSet(newObject, (string)value);

                return;
            }

            if (PropertyType == typeof(DateTime))
            {
                DateTimeSet(newObject, (DateTime)value);

                return;
            }

            if (PropertyType == typeof(Guid))
            {
                GuidSet(newObject, (Guid)value);

                return;
            }

            if (PropertyType == typeof(short))
            {
                ShortSet(newObject, (short)value);

                return;
            }

            if (PropertyType == typeof(byte))
            {
                ByteSet(newObject, (byte)value);

                return;
            }

            PropertyInfo.SetValue(newObject, value, null);
        }

        private void CreateSetMethod()
        {
            var setMethod = PropertyInfo.GetSetMethod();

            PropertyType = PropertyInfo.PropertyType;

            if (PropertyType == typeof(object))
            {
                ObjectSet = (Action<T, object>)Delegate.CreateDelegate(typeof(Action<T, object>), null, setMethod);

                return;
            }

            if (PropertyType == typeof(int))
            {
                IntSet = (Action<T, int>)Delegate.CreateDelegate(typeof(Action<T, int>), null, setMethod);


                return;
            }

            if (PropertyType == typeof(string))
            {
                StringSet = (Action<T, string>)Delegate.CreateDelegate(typeof(Action<T, string>), null, setMethod);

                return;
            }

            if (PropertyType == typeof(DateTime))
            {
                DateTimeSet = (Action<T, DateTime>)Delegate.CreateDelegate(typeof(Action<T, DateTime>), null, setMethod);


                return;
            }

            if (PropertyType == typeof(Guid))
            {
                GuidSet = (Action<T, Guid>)Delegate.CreateDelegate(typeof(Action<T, Guid>), null, setMethod);


                return;
            }

            if (PropertyType == typeof(short))
            {
                ShortSet = (Action<T, short>)Delegate.CreateDelegate(typeof(Action<T, short>), null, setMethod);

                return;
            }

            if (PropertyType == typeof(byte))
            {
                ByteSet = (Action<T, byte>)Delegate.CreateDelegate(typeof(Action<T, byte>), null, setMethod);

                return;
            }
        }
    }
}