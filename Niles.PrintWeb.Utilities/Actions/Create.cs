using System;
using CommandLine;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Utilities.Actions
{
    ///<summary>Create database options.</summary>
    [Verb("create", HelpText = "Create the DB")]
    public class CreateOptions { }

    ///<summary>Create database action.</summary>
    public class Create
    {
        ///<summary>Run create database process.</summary>
        ///<param name="logger">Logger for actions</param>
        ///<param name="settings">Database connection settings</param>
        ///<returns>0 if all is good and 1 if there was errors in creating database.</returns>
        public static int Run(ILogger logger, DatabaseConnectionSettings settings)
        {
            try
            {
                logger.LogInformation($"Try to create \"{settings.DatabaseName}\" database");

                using (var connection = DatabaseConnectionSettings.CreateServerConnection(settings))
                {
                    var comma = DatabaseConnectionSettings.CreateCommand(settings, $@"create database {settings.DatabaseName}", connection);

                    connection.Open();
                    comma.ExecuteNonQuery();
                    connection.Close();
                }

                logger.LogInformation($"{settings.DatabaseName} database successfully created");
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