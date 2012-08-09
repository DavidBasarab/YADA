using System;
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

        private void ExpectRecordRead(Options options, string commandText = CommandText)
        {
            SetUpForSingleRecordRead();

            _reader.Expect(v => v.RetrieveRecord(commandText, null, options)).Return(_mockIReader);
        }

        private void SetUpForSingleRecordRead()
        {
            _mockIReader.Expect(v => v.Read()).Return(true);

            ExpectCloseDispose();

            _mockIReader.Expect(v => v["FirstName"]).Return(FirstName);
            _mockIReader.Expect(v => v["LastName"]).Return(LastName);
        }

        [Test]
        public void CanRunReaderAsAStoreProcedure()
        {
            CreateMockReaders();

            ExpectRecordRead(Options.SingleRow | Options.StoreProcedure, StoreProcedureName);

            CreateDatabaseObject();

            var result = _database.GetRecord<SampleValue>(StoreProcedureName, options: Options.StoreProcedure);

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

            _reader.Expect(v => v.RetrieveRecord(CommandText, parameters, Options.SingleRow)).Return(_mockIReader);

            CreateDatabaseObject();

            var result = _database.GetRecord<SampleValue>(CommandText, parameters);

            MockRepository.VerifyAll();

            result.FirstName.Should().Be(FirstName);
            result.LastName.Should().Be(LastName);
        }

        [Test]
        public void ReaderWillConvertToEntity()
        {
            CreateMockReaders();
            
            ExpectRecordRead(Options.SingleRow);

            CreateDatabaseObject();

            var result = _database.GetRecord<SampleValue>(CommandText);

            Console.WriteLine("FirstName {0} | LastName {1}", result.FirstName, result.LastName);

            MockRepository.VerifyAll();

            result.FirstName.Should().Be(FirstName);
            result.LastName.Should().Be(LastName);
        }
    }
}