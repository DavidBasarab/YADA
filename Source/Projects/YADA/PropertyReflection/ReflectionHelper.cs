using System;
using YADA.Extensions;

namespace YADA.PropertyReflection
{
    internal class ReflectionHelper
    {
        public static bool operator ==(ReflectionHelper rhs, ReflectionHelper lhs)
        {
            return ObjectExtensions.IsOneNull(rhs, lhs) ? ObjectExtensions.AreBothNull(rhs, lhs) : rhs.Equals(lhs);
        }

        public static bool operator !=(ReflectionHelper rhs, ReflectionHelper lhs)
        {
            return !(rhs == lhs);
        }

        public ReflectionHelper(Type type)
        {
            ObjectType = type;
        }

        public Type ObjectType { get; set; }

        public override bool Equals(object obj)
        {
            var relfectObj = obj as ReflectionHelper;

            return obj != null && relfectObj != null && ObjectType == relfectObj.ObjectType;
        }

        public override int GetHashCode()
        {
            return ObjectType.GetHashCode();
        }
    }
}