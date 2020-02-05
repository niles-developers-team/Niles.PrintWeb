namespace Niles.PrintWeb.Models.Settings
{
    ///<summary>Model of application settings</summary>
    public class Appsettings
    {
        ///<summary>JWT token secret key</summary>
        public string Secret { get; set; }

        ///<summary>JWT Token issuer</summary>
        public string Issuer { get; set; }

        ///<summary>Application database connection settings</summary>
        public DatabaseConnectionSettings DatabaseConnectionSettings { get; set; }

        ///<summary>Application email connection settings</summary>
        public EmailConnectionSettings EmailConnectionSettings { get; set; }

        ///<summary>Application logging settings</summary>
        public Logging Logging { get; set; }        

        ///<summary>Application allowed hosts</summary>
        public string AllowedHosts { get; set; }
    }

    ///<summary>Logging settings</summary>
    public class Logging
    {
        ///<summary>Logging level setting</summary>
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }
}