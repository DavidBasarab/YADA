using System;
using System.Collections.Generic;
using YADA.Extensions;

namespace YADA.PropertyReflection
{
    internal class ReflectionHelper<T>
    {
        public static bool operator ==(ReflectionHelper<T> rhs, ReflectionHelper<T> lhs)
        {
            return ObjectExtensions.IsOneNull(rhs, lhs) ? ObjectExtensions.AreBothNull(rhs, lhs) : rhs.Equals(lhs);
        }

        public static bool operator !=(ReflectionHelper<T> rhs, ReflectionHelper<T> lhs)
        {
            return !(rhs == lhs);
        }

        private IList<PropertySetting<T>> _properties;

        public ReflectionHelper(Type type)
        {
            ObjectType = type;

            CreateProperties();
        }

        private void CreateProperties()
        {
            var properties = ObjectType.GetProperties();

            foreach(var propertyInfo in properties) Properties.Add(new PropertySetting<T>(propertyInfo));
        }

        public Type ObjectType { get; set; }

        public IList<PropertySetting<T>> Properties
        {
            get { return _properties ?? (_properties = new List<PropertySetting<T>>()); }
        }

        public override bool Equals(object obj)
        {
            var relfectObj = obj as ReflectionHelper<T>;

            return obj != null && relfectObj != null && ObjectType == relfectObj.ObjectType;
        }

        public override int GetHashCode()
        {
            return ObjectType.GetHashCode();
        }
    }
}