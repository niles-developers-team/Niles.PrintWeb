using System;
using System.IO;
using CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Shared;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("reset", HelpText = "Reset the DB (drop, create, migrate, seed)")]
    public class ResetOptions : SolutionSettingsOptions { }

    public class Reset
    {
        public static int Run(
            ILogger logger,
            Appsettings appsettings,
            ResetOptions options
        )
        {
            try
            {
                bool databaseInitialized = appsettings.DatabaseConnectionSettings != null;
                if (databaseInitialized)
                {
                    logger.LogInformation($"Try to reset \"{appsettings.DatabaseConnectionSettings.DatabaseName}\" database");
                }
                else
                {
                    logger.LogInformation($"Try to initialize project database");
                }

                SettingsUpdate.Run(logger, appsettings, options);

                appsettings = JsonConvert.DeserializeObject<Appsettings>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")));

                if (databaseInitialized)
                    if (Drop.Run(logger, appsettings.DatabaseConnectionSettings) > 0) throw new Exception("There was some errors with dropping database");
                if (Create.Run(logger, appsettings.DatabaseConnectionSettings) > 0) throw new Exception("There was some errors with creating database");
                if (MigrateUp.Run(logger, appsettings.DatabaseConnectionSettings) > 0) throw new Exception("There was some errors with migrating database");

                logger.LogInformation($"{appsettings.DatabaseConnectionSettings.DatabaseName} database successfully reseted");
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