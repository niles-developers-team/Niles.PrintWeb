using System;
using CommandLine;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("migrate-up", HelpText = "Migrate the DB schema to the latest version")]
    public class MigrateUpOptions { }
    
    public class MigrateUp
    {
        public static int Run(ILogger logger, DatabaseConnectionSettings settings)
        {
            try
            {
                logger.LogInformation($"Try to migrate \"{settings.DatabaseName}\" database");

                var serviceProvider = MigrateUtilities.CreateServices(settings);
                using (var scope = serviceProvider.CreateScope())
                {
                    // Instantiate the runner
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                    // Execute the migrations
                    runner.MigrateUp();
                }

                logger.LogInformation($"{settings.DatabaseName} database successfully migrated");
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