using System.Data;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using UnitTests.Values;
using YADA;

namespace UnitTests
{
    [TestFixture]
    [Category("Basic_DataBase_Operations")]
    internal class SingleEntityDataBaseOperations : BaseDatabaseOperationTests
    {
        private const string FirstName = "David";
        private const string LastName = "Basarab";

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

            var result = _database.GetRecord<SampleValue>(StoreProcedureName, parameters);

            MockRepository.VerifyAll();

            result.FirstName.Should().Be(FirstName);
            result.LastName.Should().Be(LastName);
        }

        private void SetUpForSingleRecordRead()
        {
            _mockIReader.Expect(v => v.Read()).Return(true);
            
            ExpectCloseDispose();

            _mockIReader.Expect(v => v["FirstName"]).Return(FirstName);
            _mockIReader.Expect(v => v["LastName"]).Return(LastName);
        }
    }
}