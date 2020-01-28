using System;
using Niles.PrintWeb.Shared;
using Microsoft.Extensions.Logging;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using System.Reflection;
using Niles.PrintWeb.Utilities.Migrations;
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Models.Settings.Enumerations;

namespace Niles.PrintWeb.Utilities.Actions
{
    public class MigrateUtilities
    {        
        public static IServiceProvider CreateServices(DatabaseConnectionSettings settings)
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    switch (settings.Provider)
                    {
                        default:
                        case DatabaseProvider.SqlServer:
                            // Add SqlServer support to FluentMigrator
                            rb.AddSqlServer()
                            .WithGlobalConnectionString(settings.SqlServerDatabaseConnectionString);
                            break;
                        case DatabaseProvider.Postgres:
                            // Add Postgres support to FluentMigrator
                            rb.AddPostgres()
                            .WithGlobalConnectionString(settings.PostgresDatabaseConnectionString);
                            break;

                    }
                    // Define the assembly containing the migrations                    
                    rb.ScanIn(typeof(CreateUser).Assembly).For.Migrations();
                })
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }
    }
}