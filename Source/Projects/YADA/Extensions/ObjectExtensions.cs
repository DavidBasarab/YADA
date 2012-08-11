using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace YADA.Extensions
{
    internal static class ObjectExtensions
    {
        public static bool AreBothNull<T>(T rhs, T lhs)
        {
            return ReferenceEquals(rhs, null) && ReferenceEquals(lhs, null);
        }

        public static TAttribute FindAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : class
        {
            var columnAttributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), false);

            TAttribute columnAttribute = null;

            if (columnAttributes.Length > 0) columnAttribute = columnAttributes[0] as TAttribute;

            return columnAttribute;
        }

        public static TAttribute FindAttributeFromType<T, TAttribute>() where TAttribute : class
        {
            var procAttributes = typeof(T).GetCustomAttributes(typeof(TAttribute), false);

            TAttribute procedureAttribute = null;

            if (procAttributes.Length > 0) procedureAttribute = procAttributes[0] as TAttribute;

            return procedureAttribute;
        }

        public static IEnumerable<TAttribute> FindAttributes<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : class
        {
            return propertyInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }

        public static bool IsOneNull<T>(T rhs, T lhs)
        {
            return ReferenceEquals(rhs, null) || ReferenceEquals(lhs, null);
        }

        public static bool OneValueNull(object compare2, object compare1)
        {
            return !AreBothNull(compare1, compare2) && IsOneNull(compare1, compare2);
        }

        public static void PrintSerializedObject<T>(this T value, ConsoleColor color = ConsoleColor.Yellow)
        {
            var document = XDocument.Parse(value.SerializeObject());

            Console.WriteLine(document.ToString(SaveOptions.OmitDuplicateNamespaces));
        }

        public static string SerializeObject<T>(this T value)
        {
            string serializedObject;

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));

                serializer.WriteObject(memoryStream, value);

                memoryStream.Position = 0;

                using (var reader = new StreamReader(memoryStream)) serializedObject = reader.ReadToEnd();
            }

            return serializedObject;
        }
    }
}