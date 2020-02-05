using System;
using Microsoft.Extensions.Logging;
using CommandLine;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Utilities.Actions
{
    ///<summary>Drop database options</summary>
    [Verb("drop", HelpText = "Drop the DB")]
    public class DropOptions { }
    
    ///<summary>Drop database action</summary>
    public class Drop
    {
        ///<summary>Run drop database process.</summary>
        ///<param name="logger">Logger for actions</param>
        ///<param name="settings">Database connection settings</param>
        ///<returns>0 if all is good and 1 if there was errors in creating database.</returns>
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