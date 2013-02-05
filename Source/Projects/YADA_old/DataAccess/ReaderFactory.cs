//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace YADA.DataAccess
//{
//    internal class ReaderFactory
//    {
//        public ReaderFactory(YADAReader reader)
//        {
//            Reader = reader;
//        }

//        public ReaderFactory()
//        {
            
//        }

//        private YADAReader _reader;
//        public YADAReader Reader
//        {
//            get { return _reader ?? (_reader = new ADOReader()); }
//        }

//        public YADAReader GetReader(string storeProcedure, IEnumerable<Parameter> parameters)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
