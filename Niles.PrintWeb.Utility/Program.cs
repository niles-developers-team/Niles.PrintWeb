using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Utility.Actions;

namespace Niles.PrintWeb.Utility
{
    class Program
    {
        static int Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return CommandLine.Parser.Default.ParseArguments<MigrateOptions, DropOptions, CreateOptions, ResetOptions, ApplicationSettingsOptions>(args)
            .MapResult(
                (DropOptions options) => RunDrop(serviceProvider.GetService<ILogger<Drop>>(), options),
                (ResetOptions options) => RunReset(serviceProvider.GetService<ILogger<Reset>>(), options),
                (CreateOptions options) => RunCreate(serviceProvider.GetService<ILogger<Create>>(), options),
                (MigrateOptions options) => RunMigrate(serviceProvider.GetService<ILogger<Migrate>>(), options),
                (ApplicationSettingsOptions options) => RunSettingsUpdate(serviceProvider.GetService<ILogger<SettingsUpdate>>(), options),
                errs => 1
            );
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole());
        }

        static int RunDrop(ILogger logger, DropOptions options) => Drop.Run(logger);
        static int RunReset(ILogger logger, ResetOptions options) => Reset.Run(logger, options);
        static int RunCreate(ILogger logger, CreateOptions options) => Create.Run(logger);
        static int RunMigrate(ILogger logger, MigrateOptions options) => Migrate.Run(logger);
        static int RunSettingsUpdate(ILogger logger, ApplicationSettingsOptions options) => SettingsUpdate.Run(logger, options);//options.DatabaseHost, options.DatabaseName, options.DatabasePort, options.DatabaseUserName, options.DatabasePassword);
    }
}
