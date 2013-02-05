using System;
using System.Collections.Generic;

namespace Yada
{
    internal class MappingInfo
    {
        public static MappingInfo Create<T>()
        {
            var mappingInfo = new MappingInfo(typeof(T));

            return mappingInfo;
        }

        public static MappingInfo Create<T>(string procedureName)
        {
            var mappingInfo = new MappingInfo(typeof(T), procedureName);

            return mappingInfo;
        }

        private MappingInfo(Type type)
        {
            DomainType = type;
            Properties = new List<PropertyMappingInfo>();
        }

        public MappingInfo(Type type, string procedureName)
            : this(type)
        {
            ProcedureName = procedureName;
        }

        public Type DomainType { get; set; }

        public IList<PropertyMappingInfo> Properties { get; set; }

        public bool MultiResultSet { get; set; }

        public string ProcedureName { get; set; }

        public MappingExpression<T> GetMappingExpression<T>()
        {
            return new MappingExpression<T>(this);
        }
    }
}