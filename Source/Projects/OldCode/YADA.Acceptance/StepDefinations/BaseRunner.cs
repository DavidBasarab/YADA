using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace YADA.Acceptance.StepDefinations
{
    internal abstract class BaseRunner
    {
        protected string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings[0].ConnectionString; }
        }

        protected void RunScriptAgainistDatabase(string scriptLocation)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var script = File.ReadAllText(scriptLocation);

                connection.Open();

                var scripts = Regex.Split(script, "\nGO");

                foreach (var item in scripts)
                {
                    using (var cmd = new SqlCommand(item, connection)) cmd.ExecuteNonQuery(); 
                }

                connection.Close();
            }
        }
    }
}