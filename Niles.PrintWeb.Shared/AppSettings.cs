using System;

namespace Niles.PrintWeb.Shared
{
    public static class AppSettings
    {
        #region Environment variables names
        public const string DatabaseHostVariableName = "database-host";

        public const string DatabaseNameVariableName = "database-name";

        public const string DatabasePortVariableName = "database-port";

        public const string DatabaseUserNameVariableName = "database-user-name";

        public const string DatabasePasswordVariableName = "database-password";
        #endregion

        public static string PostgresServerConnectionString => $"host={DatabaseHost};port={DatabasePort};username={DatabaseUserName};password={DatabasePassword}";

        public static string MSSqlServerConnectionString => $"data source={DatabaseHost};user id={DatabaseUserName};password={DatabasePassword};";

        public static string MSSqlServerDatabaseConnectionString => $"{MSSqlServerConnectionString};initial catalog={DatabaseName};"; 

        public static string PostgresDatabaseConnectionString => $"{PostgresServerConnectionString};database={DatabaseName};";

        public static string DatabaseHost => Environment.GetEnvironmentVariable(DatabaseHostVariableName, EnvironmentVariableTarget.User);

        public static string DatabaseName => Environment.GetEnvironmentVariable(DatabaseNameVariableName, EnvironmentVariableTarget.User);

        public static string DatabasePort => Environment.GetEnvironmentVariable(DatabasePortVariableName, EnvironmentVariableTarget.User);

        public static string DatabaseUserName => Environment.GetEnvironmentVariable(DatabaseUserNameVariableName, EnvironmentVariableTarget.User);

        public static string DatabasePassword => Environment.GetEnvironmentVariable(DatabasePasswordVariableName, EnvironmentVariableTarget.User);

        public static void SetAppSetting(string settingName, string value)
        {
            Environment.SetEnvironmentVariable(settingName, value, EnvironmentVariableTarget.User);
        }
    }
}