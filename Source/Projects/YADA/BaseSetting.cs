using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Yada
{
    internal abstract class BaseSetting<T> where T : class, new()
    {
        protected readonly MappingInfo mappingInfo;
        protected readonly Type type;

        protected BaseSetting(IDataReader reader)
        {
            Reader = reader;
            type = typeof(T);

            mappingInfo = Mapper.DatabaseOutputMappingInfo[type];

            CreateOrdinalMap();
        }

        protected BaseSetting(IDataReader reader, IDictionary<string, int> ordinalMap)
        {
            Reader = reader;
            OrdinalMap = ordinalMap;
        }

        protected IDictionary<string, int> OrdinalMap { get; set; }
        protected IDataReader Reader { get; set; }

        protected void CreateOrdinalMap()
        {
            OrdinalMap = new ConcurrentDictionary<string, int>();

            for (var i = 0; i < Reader.FieldCount; i++)
                OrdinalMap.Add(Reader.GetName(i), i);
        }
    }
}