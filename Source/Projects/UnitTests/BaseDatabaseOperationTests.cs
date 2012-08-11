using System.Data;
using NUnit.Framework;
using Rhino.Mocks;
using YADA;
using YADA.DataAccess;

namespace UnitTests
{
    internal abstract class BaseDatabaseOperationTests
    {
        protected const string CommandText = "SELECT * FROM FakeSQL";
        protected const string StoreProcedureName = "dbo.SomeStoreProcedure";
        protected Reader _reader;
        protected IDataReader _mockIReader;
        protected Database _database;
        public MockRepository MockRepository { get; set; }

        protected void CreateDatabaseObject()
        {
            MockRepository.ReplayAll();

            _database = new Database(_reader);
        }

        protected void CreateMockReaders()
        {
            _reader = MockRepository.StrictMock<Reader>();
            _mockIReader = MockRepository.StrictMock<IDataReader>();
        }

        [SetUp]
        public void SetUp()
        {
            MockRepository = new MockRepository();
        }

        protected void ExpectCloseDispose()
        {
            _mockIReader.Expect(v => v.Close());
            _mockIReader.Expect(v => v.Dispose());
        }

        protected void ExpectColumnAccess(string returnValue, int ordinal)
        {
            _mockIReader.Expect(v => v[ordinal]).Return(returnValue);
        }
    }
}