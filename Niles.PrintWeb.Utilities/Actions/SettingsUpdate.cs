using System;
using CommandLine;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Shared;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("app-settings", HelpText = "Set application settings by it names")]
    public class ApplicationSettingsOptions
    {
        [Option(SolutionSettings.DatabaseHostVariableName, HelpText = "Allow to set database host")]
        public string DatabaseHostOption { get; set; }
        
        [Value(0, HelpText = "Allow to set database host")]
        public string DatabaseHost { get; set; }

        [Option(SolutionSettings.DatabaseNameVariableName, HelpText = "Allow to set database name")]
        public string DatabaseNameOption { get; set; }

        [Value(1, HelpText = "Allow to set database name")]
        public string DatabaseName { get; set; }

        [Option(SolutionSettings.DatabasePortVariableName, HelpText = "Allow to set database port", Required = false)]
        public string DatabasePortOption { get; set; }

        [Value(2, HelpText = "Allow to set database port", Required = false)]
        public string DatabasePort { get; set; }

        [Option(SolutionSettings.DatabaseUserNameVariableName, HelpText = "Allow to set database user name")]
        public string DatabaseUserNameOption { get; set; }

        [Value(3, HelpText = "Allow to set database user name")]
        public string DatabaseUserName { get; set; }

        [Option(SolutionSettings.DatabasePasswordVariableName, HelpText = "Allow to set database password")]
        public string DatabasePasswordOption { get; set; }

        [Value(4, HelpText = "Allow to set database password")]
        public string DatabasePassword { get; set; }

        public void InitialiazeSettings()
        {
            DatabaseHost = @"localhost\sqlexpress";
            DatabaseName = "PrintWeb";
            DatabasePort = "5432";
            DatabaseUserName = "sa";
            DatabasePassword = "qwerty_123";
        }
    }

    public class SettingsUpdate
    {
        public static int Run(
            ILogger logger,
            ApplicationSettingsOptions options
        )
        {
            try
            {
                if (string.IsNullOrEmpty(options.DatabaseHost)
                && string.IsNullOrEmpty(options.DatabaseName)
                && string.IsNullOrEmpty(options.DatabaseUserName)
                && string.IsNullOrEmpty(options.DatabasePassword))
                {
                    options.InitialiazeSettings();
                }
                logger.LogInformation("Try to update application settings");

                SolutionSettings.SetAppSetting(SolutionSettings.DatabaseHostVariableName, options.DatabaseHost);
                SolutionSettings.SetAppSetting(SolutionSettings.DatabaseNameVariableName, options.DatabaseName);
                SolutionSettings.SetAppSetting(SolutionSettings.DatabasePortVariableName, options.DatabasePort);
                SolutionSettings.SetAppSetting(SolutionSettings.DatabaseUserNameVariableName, options.DatabaseUserName);
                SolutionSettings.SetAppSetting(SolutionSettings.DatabasePasswordVariableName, options.DatabasePassword);

                logger.LogInformation("Application settings updated successfully");
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }
            return 0;
        }
    }
}