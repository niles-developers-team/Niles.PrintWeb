using System;
using Niles.PrintWeb.Shared;
using Microsoft.Extensions.Logging;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using System.Reflection;
using Niles.PrintWeb.Utility.Migrations;

namespace Niles.PrintWeb.Utility.Actions
{
    [Verb("migrate", HelpText = "Migrate the DB schema to the latest version")]
    class MigrateOptions { }
    public class Migrate
    {
        public static int Run(ILogger logger)
        {
            try
            {
                logger.LogInformation($"Try to drop \"{SolutionSettings.DatabaseName}\" database");

                var serviceProvider = CreateServices();
                using (var scope = serviceProvider.CreateScope())
                {
                    UpdateDatabase(scope.ServiceProvider);
                }

                logger.LogInformation($"{SolutionSettings.DatabaseName} database successfully dropped");
                return 0;
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
                return 1;
            }
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add Postgres support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString(SolutionSettings.MSSqlServerDatabaseConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateUser).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}