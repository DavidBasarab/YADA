using System;
using System.Configuration;
using System.Data.SqlClient;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace YADA.Acceptance.StepDefinations
{
    [Binding]
    internal class Hookup : BaseRunner
    {
        private static void WriteErrorToConsole(Exception exception, int tabNumber = 0)
        {
            var spaces = new string(' ', tabNumber * 4);

            Console.WriteLine("");
            Console.WriteLine("{0}{1} EXCEPTION {1}", spaces, new string('=', 30));
            Console.WriteLine("");
            Console.WriteLine("{0}    MESSAGE    : {1}", spaces, exception.Message);

            Console.WriteLine("{0}    STACKTRACE : {1}", spaces,
                              exception.StackTrace.Replace(Environment.NewLine, string.Format("{1}{0}", new string(' ', (tabNumber * 4) + 16), Environment.NewLine)));

            Console.WriteLine("{0}{1}", spaces, new string('-', 71));

            if (exception.InnerException != null) WriteErrorToConsole(exception, tabNumber + 1);
        }

        private bool Connected { get; set; }
        private bool ReadAdventureWorksDatabase { get; set; }

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

        [When(@"I attempt to connect to the database")]
        public void WhenIAttemptToConnectToTheDatabase()
        {
            try
            {
                var connection = new SqlConnection(ConnectionString);

                connection.Open();
                connection.Close();
                connection.Dispose();

                Connected = true;
            }
            catch (Exception ex)
            {
                Connected = false;

                WriteErrorToConsole(ex);
            }
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

                WriteErrorToConsole(ex);
            }
        }
    }
}