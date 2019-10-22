using System;
using Niles.PrintWeb.Shared;
using Microsoft.Extensions.Logging;
using CommandLine;
using System.Data.SqlClient;

namespace Niles.PrintWeb.Utility.Actions
{
    [Verb("drop", HelpText = "Drop the DB")]
    class DropOptions { }
    
    public class Drop
    {
        public static int Run(ILogger logger)
        {
            try
            {
                logger.LogInformation($"Try to drop \"{AppSettings.DatabaseName}\" database");

                using (var connection = new SqlConnection(AppSettings.MSSqlServerConnectionString))
                {
                    var comma = new SqlCommand($"drop database if exists {AppSettings.DatabaseName}", connection);

                    connection.Open();
                    comma.ExecuteNonQuery();
                    connection.Close();
                }
                
                logger.LogInformation($"{AppSettings.DatabaseName} database successfully dropped");
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