using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using TechTalk.SpecFlow;
using YADA.Acceptance.Extensions;
using YADA.Acceptance.StepDefinations.Values;

namespace YADA.Acceptance.StepDefinations
{
    [Binding]
    internal class SmallTableReading : BaseRunner
    {
        private IList<int> _executionTimes;
        private TimeSpan ExecutionTime { get; set; }

        private IList<int> ExecutionTimes
        {
            get { return _executionTimes ?? (_executionTimes = new List<int>()); }
            set { _executionTimes = value; }
        }

        public double AverageExecutionTime
        {
            get { return ExecutionTimes.Average(); }
        }

        private int NumberOfInsertedRows { get; set; }

        [Given(@"I can connect to AdventureWorks database")]
        public void GivenICanConnectToAdventureWorksDatabase()
        {
            var testResult = Helpers.WhenIAttemptToConnectToTheDatabase();

            testResult.Should().BeTrue();
        }

        [Given(@"I have a table populated with between 10000 to 20000 rows")]
        public void GivenIHaveATablePopulatedWithBetween10000To20000Rows()
        {
            var value = GetScalarValue("SELECT COUNT(1) FROM Sales.Customer");

            value.Should().BeInRange(10000, 20000);
        }

        [Given(@"I have a table populated with between 500 to 1000 rows")]
        public void GivenIHaveATablePopulatedWithBetween500To1000Rows()
        {
            var value = GetScalarValue("SELECT COUNT(1) FROM Sales.Store");

            value.Should().BeInRange(500, 1000);
        }

        [Given(@"I have a table populated with over 100000 rows")]
        public void GivenIHaveATablePopulatedWithOver100000Rows()
        {
            var value = GetScalarValue("SELECT COUNT(1) FROM Sales.SalesOrderDetail");

            value.Should().BeGreaterThan(100000);
        }

        [Then(@"the operation should happen in less than (.*) ms")]
        public void ThenTheOperationShouldHappenInLessThanMS(int milliseconds)
        {
            AverageExecutionTime.Should().BeLessThan(milliseconds);
        }

        [When(@"using a store procedure \((.*)\) to read the records")]
        public void WhenUsingAStoreProcedureTestToReadTheRecords(string procedureName)
        {
            var database = Database.Instance;

            for (var i = 0; i < 50; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                switch (procedureName)
                {
                    case "Sales.SmallRowTest":
                        database.GetRecords<Store>(procedureName);
                        break;
                    case "Sales.MediumRowTest":
                        database.GetRecords<Customer>(procedureName);
                        break;
                    case "Sales.LargeRowTest":
                        database.GetRecords<SalesOrderDetail>(procedureName);
                        break;
                }

                stopwatch.Stop();

                ExecutionTime = stopwatch.Elapsed;

                ExecutionTimes.Add(ExecutionTime.Milliseconds);
            }

            Console.WriteLine("Average Read Time for read {0} MS", AverageExecutionTime);
        }

        [When(@"using a store procedure to read a record")]
        public void WhenUsingAStoreProcedureToReadARecord()
        {
            var database = Database.Instance;

            var vendorIDs = new[]
                            {
                                1500,
                                1502,
                                1504
                            };

            for (var i = 0; i < 100; i++)
            {
                var stopWatch = Stopwatch.StartNew();

                var keyID = (i % 3);

                var item = database.GetRecord<Vendor>("Purchasing.GetVendorByVendorID", new[] { Parameter.Create("BusinessEntityID", vendorIDs[keyID]) });

                stopWatch.Stop();

                ExecutionTime = stopWatch.Elapsed;

                ExecutionTimes.Add(ExecutionTime.Milliseconds);

                switch (keyID)
                {
                    case 0:
                        item.BusinessEntityID.Should().Be(1500);
                        item.AccountNumber.Should().Be("MORGANB0001");
                        item.Name.Should().Be("Morgan Bike Accessories");
                        item.CreditRating.Should().Be(1);
                        item.PreferredVendorStatus.Should().BeTrue();
                        item.ActiveFlag.Should().BeTrue();
                        item.PurchasingWebServiceURL.Should().BeNullOrEmpty();
                        item.ModifiedDate.Should().Be(DateTime.Parse("2006-03-05"));
                        break;
                    case 1:
                        item.BusinessEntityID.Should().Be(1502);
                        item.AccountNumber.Should().Be("CYCLING0001");
                        item.Name.Should().Be("Cycling Master");
                        item.CreditRating.Should().Be(1);
                        item.PreferredVendorStatus.Should().BeTrue();
                        item.ActiveFlag.Should().BeTrue();
                        item.PurchasingWebServiceURL.Should().BeNullOrEmpty();
                        item.ModifiedDate.Should().Be(DateTime.Parse("2006-01-24"));
                        break;
                    default:
                        item.BusinessEntityID.Should().Be(1504);
                        item.AccountNumber.Should().Be("CHICAGO0002");
                        item.Name.Should().Be("Chicago Rent-All");
                        item.CreditRating.Should().Be(2);
                        item.PreferredVendorStatus.Should().BeTrue();
                        item.ActiveFlag.Should().BeTrue();
                        item.PurchasingWebServiceURL.Should().BeNullOrEmpty();
                        item.ModifiedDate.Should().Be(DateTime.Parse("2006-01-24"));
                        break;
                }
            }

            Console.WriteLine("Average Read Time for read {0} MS", AverageExecutionTime);
        }

        private int GetScalarValue(string commandText)
        {
            try
            {
                var value = 0;

                using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    value = (int)command.ExecuteScalar();
                    connection.Close();
                }

                return value;
            }
            catch (Exception ex)
            {
                Helpers.WriteErrorToConsole(ex);

                return -1;
            }
        }

        //[When(@"using a store procedure to read in (.*) records")]
        //public void WhenUsingAStoreProcedureToReadInRecords(int numberOfRecords)
        //{
        //    var database = Database.Instance;

        //    for (var i = 0; i < 50; i++)
        //    {
        //        var startRecordID = NumberExtensions.NextRandom(1, NumberOfInsertedRows - numberOfRecords - 1);

        //        var parameters = new[]
        //                         {
        //                             Parameter.Create("MinRecordID", startRecordID),
        //                             Parameter.Create("MaxRecordID", startRecordID + numberOfRecords - 1)
        //                         };

        //        var stopwatch = Stopwatch.StartNew();

        //        var items = database.GetRecords<NarrowSmallData>("[YadaTesting].[dbo].[GetRangeOfRecords]", parameters);

        //        stopwatch.Stop();

        //        ExecutionTime = stopwatch.Elapsed;

        //        ExecutionTimes.Add(ExecutionTime.Milliseconds);

        //        items.Count.Should().Be(numberOfRecords);
        //    }

        //    Console.WriteLine("Average Read Time for read {0} MS", AverageExecutionTime);
        //}
    }
}