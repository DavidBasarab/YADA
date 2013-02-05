namespace YADA
{
    internal class ConfigurationManager
    {
        public static string ConnectionString
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings[0].ToString(); }
        }
    }
}