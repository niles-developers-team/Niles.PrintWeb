using System;
using Microsoft.Extensions.Logging;
using CommandLine;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("drop", HelpText = "Drop the DB")]
    public class DropOptions { }
    
    public class Drop
    {
        public static int Run(ILogger logger, DatabaseConnectionSettings settings)
        {
            try
            {
                logger.LogInformation($"Try to drop \"{settings.DatabaseName}\" database");

                using (var connection = DatabaseConnectionSettings.CreateServerConnection(settings))
                {
                    var comma = DatabaseConnectionSettings.CreateCommand(settings, $"drop database if exists {settings.DatabaseName}", connection);

                    connection.Open();
                    comma.ExecuteNonQuery();
                    connection.Close();
                }
                
                logger.LogInformation($"{settings.DatabaseName} database successfully dropped");
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