using System.Data;
using System.Data.SqlClient;
using Niles.PrintWeb.Models.Settings.Enumerations;
using Npgsql;

namespace Niles.PrintWeb.Models.Settings
{
    public class DatabaseConnectionSettings
    {
        #region Private variables
        private string databaseHost;
        private string databaseName;
        private string databasePort;
        private string databaseUserName;
        private string databasePassword;
        private DatabaseProvider? provider;
        #endregion

        #region Environment variables names
        public const string DatabaseHostVariableName = "database-host";

        public const string DatabaseNameVariableName = "database-name";

        public const string DatabasePortVariableName = "database-port";

        public const string DatabaseUserNameVariableName = "database-user-name";

        public const string DatabasePasswordVariableName = "database-password";

        public const string DatabaseProviderVariableName = "database-provider";
        #endregion

        public string PostgresConnectionString { get => $"host={databaseHost};port={databasePort};username={databaseUserName};password={databasePassword}"; }

        public string SqlServerConnectionString { get => $"data source={databaseHost};user id={databaseUserName};password={databasePassword}"; }

        public string SqlServerDatabaseConnectionString { get => $"{SqlServerConnectionString};initial catalog={databaseName};"; }

        public string PostgresDatabaseConnectionString { get => $"{PostgresConnectionString};database={databaseName};"; }

        public string DatabaseHost { get => databaseHost; set => databaseHost = value; }
        public string DatabaseName { get => databaseName; set => databaseName = value; }
        public string DatabasePort { get => databasePort; set => databasePort = value; }
        public string DatabaseUserName { get => databaseUserName; set => databaseUserName = value; }
        public string DatabasePassword { get => databasePassword; set => databasePassword = value; }
        public DatabaseProvider? Provider { get => provider; set => provider = value; }

        public static DatabaseConnectionSettings InitializeSolutionSettings(
            string databaseHost,
            string databaseName,
            string databasePort,
            string databaseUserName,
            string databasePassword,
            DatabaseProvider provider
        )
        {
            return new DatabaseConnectionSettings
            {
                databaseHost = databaseHost,
                databaseName = databaseName,
                databasePassword = databasePassword,
                databasePort = databasePort,
                databaseUserName = databaseUserName,
                provider = provider
            };
        }

        public static IDbCommand CreateCommand(DatabaseConnectionSettings settings, string command, IDbConnection connection)
        {
            switch (settings.Provider)
            {
                case DatabaseProvider.Postgres: return new NpgsqlCommand(command, connection as NpgsqlConnection);
                case DatabaseProvider.SqlServer: return new SqlCommand(command, connection as SqlConnection);
                default: return new SqlCommand(command, connection as SqlConnection);
            }
        }

        public static IDbConnection CreateServerConnection(DatabaseConnectionSettings settings)
        {
            switch (settings.Provider)
            {
                default:
                case DatabaseProvider.SqlServer: return new SqlConnection(settings.SqlServerConnectionString);
                case DatabaseProvider.Postgres: return new NpgsqlConnection(settings.PostgresConnectionString);
            }
        }

        public static IDbConnection CreateDatabaseConnection(DatabaseConnectionSettings settings)
        {
            switch (settings.Provider)
            {
                default:
                case DatabaseProvider.SqlServer: return new SqlConnection(settings.SqlServerDatabaseConnectionString);
                case DatabaseProvider.Postgres: return new NpgsqlConnection(settings.PostgresDatabaseConnectionString);
            }
        }
    }
}