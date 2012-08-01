using System;
using System.Configuration;
using System.Data.SqlClient;

namespace YADA.Acceptance.StepDefinations
{
    public static class Helpers
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings[0].ConnectionString; }
        }

        public static bool WhenIAttemptToConnectToTheDatabase()
        {
            try
            {
                var connection = new SqlConnection(ConnectionString);

                connection.Open();
                connection.Close();
                connection.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                WriteErrorToConsole(ex);

                return false;
            }
        }

        public static void WriteErrorToConsole(Exception exception, int tabNumber = 0)
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
    }
}