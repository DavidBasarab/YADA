using System;

namespace Yada
{
    internal class ProcedureMap
    {
        public static bool operator ==(ProcedureMap rhs, ProcedureMap lhs)
        {
            if (ReferenceEquals(rhs, null) || ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null) && ReferenceEquals(lhs, null);

            return rhs.Equals(lhs);
        }

        public static bool operator !=(ProcedureMap rhs, ProcedureMap lhs)
        {
            if (ReferenceEquals(rhs, null) || ReferenceEquals(lhs, null))
                return !ReferenceEquals(rhs, null) || !ReferenceEquals(lhs, null);

            return !rhs.Equals(lhs);
        }

        public ProcedureMap(string key, Type type)
        {
            Key = key;
            Type = type;
        }

        public string Key { get; set; }
        public Type Type { get; set; }

        public override bool Equals(object obj)
        {
            var procMap = obj as ProcedureMap;

            return obj != null && procMap != null && procMap.Key == Key && procMap.Type == Type;
        }

        public override int GetHashCode()
        {
            var hashKey = string.Format("{0} == {1}", Key, Type.GetHashCode());

            return hashKey.GetHashCode();
        }
    }
}