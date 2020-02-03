using System;
using System.IO;
using CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Models.Settings.Enumerations;

namespace Niles.PrintWeb.Utilities.Actions
{
    ///<summary>Settings update database options.</summary>
    [Verb("update-settings", HelpText = "Set application settings by it names")]
    public class SetSettingsOptions
    {
        [Option(DatabaseConnectionSettings.DatabaseHostVariableName, HelpText = "Allow to set database host")]
        public string DatabaseHost { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseNameVariableName, HelpText = "Allow to set database name")]
        public string DatabaseName { get; set; }

        [Option(DatabaseConnectionSettings.DatabasePortVariableName, HelpText = "Allow to set database port", Required = false)]
        public string DatabasePort { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseUserNameVariableName, HelpText = "Allow to set database user name")]
        public string DatabaseUserName { get; set; }

        [Option(DatabaseConnectionSettings.DatabasePasswordVariableName, HelpText = "Allow to set database password")]
        public string DatabasePassword { get; set; }

        [Option(DatabaseConnectionSettings.DatabaseProviderVariableName, HelpText = "Allow to set database provider")]
        public DatabaseProvider? Provider { get; set; }

        public void InitialiazeSettings()
        {
            DatabaseHost = @"localhost\sqlexpress";
            DatabaseName = "printweb";
            DatabasePort = "5432";
            DatabaseUserName = "sa";
            DatabasePassword = "qwerty_123";
            Provider = DatabaseProvider.SqlServer;
        }
    }

    ///<summary>Settings update database action.</summary>
    public class SettingsUpdate
    {
        ///<summary>Run settings update database process.</summary>
        ///<param name="logger">Logger for actions</param>
        ///<param name="settings">Database connection settings</param>
        ///<returns>0 if all is good and 1 if there was errors in creating database.</returns>
        public static int Run(
            ILogger logger,
            SetSettingsOptions options
        )
        {
            try
            {
                if (string.IsNullOrEmpty(options.DatabaseHost)
                && string.IsNullOrEmpty(options.DatabaseName)
                && string.IsNullOrEmpty(options.DatabaseUserName)
                && string.IsNullOrEmpty(options.DatabasePassword)
                && !options.Provider.HasValue)
                {
                    options.InitialiazeSettings();
                }
                logger.LogInformation("Try to update solution settings");

                var connectionSettings = DatabaseConnectionSettings.InitializeSolutionSettings(
                    options.DatabaseHost,
                    options.DatabaseName,
                    options.DatabasePort,
                    options.DatabaseUserName,
                    options.DatabasePassword,
                    options.Provider.Value
                );

                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), JsonConvert.SerializeObject(connectionSettings));

                logger.LogInformation("Solution settings updated successfully");
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }
            return 0;
        }
    }
}