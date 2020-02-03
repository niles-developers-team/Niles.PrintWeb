using System;
using System.IO;
using CommandLine;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("reset", HelpText = "Reset the DB (drop, create, migrate, seed)")]
    public class ResetOptions : SetSettingsOptions { }

    public class Reset
    {
        public static int Run(
            ILogger logger,
            DatabaseConnectionSettings connectionSettings,
            ResetOptions options
        )
        {
            try
            {
                bool databaseInitialized = connectionSettings != null;
                if (databaseInitialized)
                {
                    logger.LogInformation($"Try to reset \"{connectionSettings.DatabaseName}\" database");
                }
                else
                {
                    logger.LogInformation($"Try to initialize project database");
                }

                SettingsUpdate.Run(logger, options);

                connectionSettings = JsonConvert.DeserializeObject<DatabaseConnectionSettings>(
                    File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                );

                if (databaseInitialized)
                    if (Drop.Run(logger, connectionSettings) > 0) throw new Exception("There was some errors with dropping database");
                if (Create.Run(logger, connectionSettings) > 0) throw new Exception("There was some errors with creating database");
                if (MigrateUp.Run(logger, connectionSettings) > 0) throw new Exception("There was some errors with migrating database");

                logger.LogInformation($"{connectionSettings.DatabaseName} database successfully reseted");
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