using System;
using CommandLine;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Shared;

namespace Niles.PrintWeb.Utility.Actions
{
    [Verb("reset", HelpText = "Reset the DB (drop, create, migrate, seed)")]
    public class ResetOptions : ApplicationSettingsOptions { }

    public class Reset
    {
        public static int Run(ILogger logger, ResetOptions options)
        {
            try
            {
                logger.LogInformation($"Try to reset \"{SolutionSettings.DatabaseName}\" database");

                SettingsUpdate.Run(logger, options);
                Drop.Run(logger);
                Create.Run(logger);
                Migrate.Run(logger);

                logger.LogInformation($"{SolutionSettings.DatabaseName} database successfully reseted");
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }
    }
}