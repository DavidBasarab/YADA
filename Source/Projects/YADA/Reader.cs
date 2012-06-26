using System;

namespace YADA
{
    public class Reader<TEntity> where TEntity : class
    {
        public static TEntity GetRecord(string procedureName, int keyID)
        {
            return default(TEntity);
        }
    }
}