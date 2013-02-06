using System.Data;
using Moq;
using NUnit.Framework;

namespace YADA.UnitTests.Mapping
{
    public abstract class MappingTest
    {
        protected Mock<IDataReader> reader;

        [SetUp]
        public void SetUp()
        {
            Yada.Yada.Start();
        }

        protected void CreateReader()
        {
            reader = new Mock<IDataReader>();
        }
    }
}