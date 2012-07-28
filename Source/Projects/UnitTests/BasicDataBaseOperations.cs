using System.Data;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using UnitTests.Values;
using YADA;
using YADA.DataAccess;

namespace UnitTests
{
    [TestFixture]
    [Category("Basic_DataBase_Operations")]
    public class BasicDataBaseOperations
    {
        private Reader _reader;
        private IDataReader _mockIReader;
        private Database _database;
        private const string FirstName = "David";
        private const string LastName = "Basarab";
        private const string StoreProcedureName = "dbo.TestProcedure";
        public MockRepository MockRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockRepository = new MockRepository();
        }

        [Test]
        public void ReaderWillConvertToEntity()
        {
            CreateMockReaders();

            SetUpForSingleRecordRead();

            _reader.Expect(v => v.RetrieveRecord(StoreProcedureName, null, CommandBehavior.SingleRow)).Return(_mockIReader);

            CreateDatabaseObject();

            var result = _database.GetRecord<SampleValue>(StoreProcedureName);

            MockRepository.VerifyAll();

            result.FirstName.Should().Be(FirstName);
            result.LastName.Should().Be(LastName);
        }

        [Test]
        public void DatabaseCanAcceptParamtersForRecordRetrieval()
        {
            CreateMockReaders();

            SetUpForSingleRecordRead();

            var parameters = new[]
                             {
                                 new Parameter("ID", 17)
                             };

            _reader.Expect(v => v.RetrieveRecord(StoreProcedureName, parameters, CommandBehavior.SingleRow)).Return(_mockIReader);

            CreateDatabaseObject();

            var result = _database.GetRecord<SampleValue>(StoreProcedureName);

            MockRepository.VerifyAll();

            result.FirstName.Should().Be(FirstName);
            result.LastName.Should().Be(LastName);
        }

        private void SetUpForSingleRecordRead()
        {
            _mockIReader.Expect(v => v.Read()).Return(true);
            _mockIReader.Expect(v => v.Close());
            _mockIReader.Expect(v => v.Dispose());

            _mockIReader.Expect(v => v["FirstName"]).Return(FirstName);
            _mockIReader.Expect(v => v["LastName"]).Return(LastName);
        }

        private void CreateDatabaseObject()
        {
            MockRepository.ReplayAll();

            _database = new Database(_reader);
        }

        private void CreateMockReaders()
        {
            _reader = MockRepository.StrictMock<Reader>();
            _mockIReader = MockRepository.StrictMock<IDataReader>();
        }
    }
}