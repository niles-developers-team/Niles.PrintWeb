using System;
using System.IO;
using CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Models.Settings.Enumerations;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("update-settings", HelpText = "Set application settings by it names")]
    public class SolutionSettingsOptions
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
            DatabaseName = "andromeda";
            DatabasePort = "5432";
            DatabaseUserName = "sa";
            DatabasePassword = "qwerty_123";
            Provider = DatabaseProvider.SqlServer;
        }
    }

    public class SettingsUpdate
    {
        public static int Run(
            ILogger logger,
            Appsettings appsettings,
            SolutionSettingsOptions options
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

                appsettings.DatabaseConnectionSettings = DatabaseConnectionSettings.InitializeSolutionSettings(
                    options.DatabaseHost,
                    options.DatabaseName,
                    options.DatabasePort,
                    options.DatabaseUserName,
                    options.DatabasePassword,
                    options.Provider.Value
                );

                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), JsonConvert.SerializeObject(appsettings));

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