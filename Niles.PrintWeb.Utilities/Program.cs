using CommandLine;
using System.Linq;

using Niles.PrintWeb.Utilities.Actions;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Niles.PrintWeb.Utilities
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            DatabaseConnectionSettings settings = JsonConvert.DeserializeObject<DatabaseConnectionSettings>(
                File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
            );

            var services = serviceCollection.BuildServiceProvider();

            return CommandLine.Parser.Default.ParseArguments<MigrateUpOptions, MigrateDownOptions, DropOptions, CreateOptions, ResetOptions, SetSettingsOptions>(args)
            .MapResult(
                (DropOptions options) => RunDrop(services.GetService<ILogger<Drop>>(), settings),
                (ResetOptions options) => RunReset(services.GetService<ILogger<Reset>>(), settings, options),
                (CreateOptions options) => RunCreate(services.GetService<ILogger<Create>>(), settings),
                (MigrateUpOptions options) => RunMigrateUp(services.GetService<ILogger<MigrateUp>>(), settings),
                (MigrateDownOptions options) => RunMigrateDown(services.GetService<ILogger<MigrateDown>>(), settings),
                (SetSettingsOptions options) => RunSettingsUpdate(services.GetService<ILogger<SettingsUpdate>>(), options),
                errors => 1
            );
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(builder => builder.AddConsole());
        }

        static int RunDrop(ILogger logger, DatabaseConnectionSettings settings) => Drop.Run(logger, settings);
        static int RunReset(ILogger logger, DatabaseConnectionSettings settings, ResetOptions options) => Reset.Run(logger, settings, options);
        static int RunCreate(ILogger logger, DatabaseConnectionSettings settings) => Create.Run(logger, settings);
        static int RunMigrateUp(ILogger logger, DatabaseConnectionSettings settings) => MigrateUp.Run(logger, settings);
        static int RunMigrateDown(ILogger logger, DatabaseConnectionSettings settings) => MigrateDown.Run(logger, settings);
        static int RunSettingsUpdate(ILogger logger, SetSettingsOptions options) => SettingsUpdate.Run(logger, options);
    }
}