using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace YADA.Acceptance.StepDefinations
{
    [Binding]
    internal class Hookup : BaseRunner
    {
        private bool Connected { get; set; }
        private bool ReadAdventureWorksDatabase { get; set; }
        private bool ProcedureRecordsReturned { get; set; }

        [Given(@"I have a connection string configured")]
        public void GivenIHaveAConnectionStringConfigured()
        {
            ConfigurationManager.ConnectionStrings.Count.Should().BeGreaterThan(0);
        }

        [Then(@"I can connect to the database")]
        public void ThenICanConnectToTheDatabase()
        {
            Connected.Should().BeTrue();
        }

        [Then(@"I can get results from the database")]
        public void ThenICanGetResultsFromTheDatabase()
        {
            ReadAdventureWorksDatabase.Should().BeTrue();
        }

        [Then(@"I have records returned")]
        public void ThenIHaveRecordsReturned()
        {
            ProcedureRecordsReturned.Should().BeTrue();
        }

        [When(@"I attempt to connect to the database")]
        public void WhenIAttemptToConnectToTheDatabase()
        {
            Connected = Helpers.WhenIAttemptToConnectToTheDatabase();
        }

        [When(@"I attempt to read from an adventure works table")]
        public void WhenIAttemptToReadFromAnAdventureWorksTable()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    const string commandText = @"SELECT COUNT(1) FROM [HumanResources].[Department]";

                    using (var command = new SqlCommand(commandText, connection))
                    {
                        connection.Open();
                        var scalarValue = command.ExecuteScalar();
                        connection.Close();

                        ((int)(scalarValue)).Should().Be(16);
                    }
                }

                ReadAdventureWorksDatabase = true;
            }
            catch (Exception ex)
            {
                ReadAdventureWorksDatabase = false;

                Helpers.WriteErrorToConsole(ex);
            }
        }

        [When(@"I attempt to run (.*)")]
        public void WhenIAttemptToRun(string procedureName)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        ProcedureRecordsReturned = reader.HasRows;

                        reader.Close();
                        reader.Dispose();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ProcedureRecordsReturned = false;

                Helpers.WriteErrorToConsole(ex);
            }
        }
    }
}