using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using UnitTests.Values;
using YADA;

namespace UnitTests
{
    [TestFixture]
    [Category("Multiple_RecordsSets_DatabaseOperations")]
    internal class MultipleRecordsSetsDatabaseOperations : BaseDatabaseOperationTests
    {
        private const string ItemOneFirstName = "Stan";
        private const string ItemOneLastName = "Marsh";
        private const string ItemTwoFirstName = "Eric";
        private const string ItemTwoLastName = "Cartman";

        private void ExpectColumnAccess(string columnName, string returnValue)
        {
            _mockIReader.Expect(v => v[columnName]).Return(returnValue);
        }

        private void ExpectMultiReturnRecords()
        {
            ExpectCloseDispose();

            ExpectRead(true);
            ExpectRead(true);
            ExpectRead(false);

            ExpectColumnAccess("FirstName", ItemOneFirstName);
            ExpectColumnAccess("LastName", ItemOneLastName);
            ExpectColumnAccess("FirstName", ItemTwoFirstName);
            ExpectColumnAccess("LastName", ItemTwoLastName);
        }

        private void ExpectRead(bool returnValue)
        {
            _mockIReader.Expect(v => v.Read()).Return(returnValue);
        }

        private void VerifyMultiReturnRecords(IList<SampleValue> lists)
        {
            MockRepository.VerifyAll();

            lists.Count.Should().Be(2);

            var firstItem = lists[0];
            var secondItem = lists[1];

            firstItem.FirstName.Should().Be(ItemOneFirstName);
            firstItem.LastName.Should().Be(ItemOneLastName);

            secondItem.FirstName.Should().Be(ItemTwoFirstName);
            secondItem.LastName.Should().Be(ItemTwoLastName);
        }

        [Test]
        public void WillGetAListOfEntities()
        {
            CreateMockReaders();

            ExpectMultiReturnRecords();

            CreateDatabaseObject();

            _reader.Expect(v => v.RetrieveRecord(StoreProcedureName, null)).Return(_mockIReader);

            var lists = _database.GetRecords<SampleValue>(StoreProcedureName);

            VerifyMultiReturnRecords(lists);
        }

        [Test]
        public void WillGetAListOfEntitesWithParameters()
        {
            CreateMockReaders();

            ExpectMultiReturnRecords();

            CreateDatabaseObject();

            var paramters = new[]
                            {
                                new Parameter("MaxNumber", 1123),
                                new Parameter("MinNumber", 1)
                            };

            _reader.Expect(v => v.RetrieveRecord(StoreProcedureName, paramters)).Return(_mockIReader);

            var lists = _database.GetRecords<SampleValue>(StoreProcedureName, paramters);

            VerifyMultiReturnRecords(lists);
        }
    }
}