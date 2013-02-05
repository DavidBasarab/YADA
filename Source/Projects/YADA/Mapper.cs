using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Yada
{
    public static class Mapper
    {
        internal static IDictionary<Type, MappingInfo> DatabaseOutputMappingInfo { get; private set; }
        internal static IDictionary<ProcedureMap, MappingInfo> InputProcedureMappingInfo { get; private set; }

        public static MappingExpression<T> CreateInputMap<T>(string procedureName)
        {
            var mappingInfo = MappingInfo.Create<T>(procedureName);

            InputProcedureMappingInfo.Add(mappingInfo);

            return mappingInfo.GetMappingExpression<T>();
        }

        public static MappingExpression<T> CreateMap<T>()
        {
            var mappingInfo = MappingInfo.Create<T>();

            DatabaseOutputMappingInfo.Add(mappingInfo);

            return mappingInfo.GetMappingExpression<T>();
        }

        internal static void Reset()
        {
            DatabaseOutputMappingInfo = new Dictionary<Type, MappingInfo>();
            InputProcedureMappingInfo = new Dictionary<ProcedureMap, MappingInfo>();
        }

        internal static bool DoesColumnExist(IDataRecord reader, string columnName)
        {
            for (var i = 0; i < reader.FieldCount; i++) if (reader.GetName(i) == columnName) return true;

            return false;
        }

        internal static void MapObjects()
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) RunMaps(assembly);
        }

        private static void Add(this IDictionary<Type, MappingInfo> collection, MappingInfo mappingInfo)
        {
            if (collection.ContainsKey(mappingInfo.DomainType)) collection[mappingInfo.DomainType] = mappingInfo;
            else collection.Add(mappingInfo.DomainType, mappingInfo);
        }

        private static void Add(this IDictionary<ProcedureMap, MappingInfo> collection, MappingInfo mappingInfo)
        {
            var key = new ProcedureMap(mappingInfo.ProcedureName, mappingInfo.DomainType);

            if (collection.ContainsKey(key)) collection[key] = mappingInfo;
            else collection.Add(key, mappingInfo);
        }


        private static void RunMaps(Assembly assembly)
        {
            var objectMap = typeof(Map);

            var mapTypes = assembly
                .GetTypes()
                .Where(p => objectMap.IsAssignableFrom(p) && p.IsClass);

            foreach(var t in mapTypes)
            {
                var instance = (Map)Activator.CreateInstance(t);
                instance.CreateMap();
            }
        }

        static Mapper()
        {
            Reset();
        }
    }
}