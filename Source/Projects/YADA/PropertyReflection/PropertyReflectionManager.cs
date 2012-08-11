using System;
using System.Collections.Generic;

namespace YADA.PropertyReflection
{
    internal class PropertyReflectionManager
    {
        private static IDictionary<Type, ReflectionHelper> Cache { get; set; }

        public PropertyReflectionManager(IDictionary<Type, ReflectionHelper> cache)
        {
            Cache = cache;
        }

        public PropertyReflectionManager() {}

        public ReflectionHelper GetFromCache(Type type)
        {
            ReflectionHelper helper;

            if (Cache.TryGetValue(type, out helper)) return helper;

            helper = new ReflectionHelper(type);

            Cache.Add(type, helper);

            return helper;
        }
    }
}