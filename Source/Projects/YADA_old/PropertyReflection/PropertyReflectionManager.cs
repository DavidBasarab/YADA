using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YADA.PropertyReflection
{
    internal class PropertyReflectionManager
    {
        private static IDictionary<Type, object> _cache;

        private static IDictionary<Type, object> Cache
        {
            get { return _cache ?? (_cache = new ConcurrentDictionary<Type, object>()); }
        }

        public PropertyReflectionManager(IDictionary<Type, object> cache)
        {
            _cache = cache;
        }

        public PropertyReflectionManager() {}

        public ReflectionHelper<T> GetFromCache<T>()
        {
            var type = typeof(T);
            
            object helper;

            if (Cache.TryGetValue(type, out helper)) return (ReflectionHelper<T>)helper;

            helper = new ReflectionHelper<T>(type);

            Cache.Add(type, helper);

            return (ReflectionHelper<T>)helper;
        }
    }
}