using System.Collections.Generic;
using System.Data;

namespace Yada
{
    public static class Data
    {
        public static T FromDataRecord<T>(IDataReader reader) where T : class, new()
        {
            return Creator<T>.Create(reader);
        }

        public static IList<T> ListFromDataRecord<T>(IDataReader reader) where T : class, new()
        {
            return Creator<T>.CreateList(reader);
        }
    }
}