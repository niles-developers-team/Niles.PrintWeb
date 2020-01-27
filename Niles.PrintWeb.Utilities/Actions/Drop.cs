using System;
using Niles.PrintWeb.Shared;
using Microsoft.Extensions.Logging;
using CommandLine;
using System.Data.SqlClient;

namespace Niles.PrintWeb.Utilities.Actions
{
    [Verb("drop", HelpText = "Drop the DB")]
    class DropOptions { }
    
    public class Drop
    {
        public static int Run(ILogger logger)
        {
            try
            {
                logger.LogInformation($"Try to drop \"{SolutionSettings.DatabaseName}\" database");

                using (var connection = new SqlConnection(SolutionSettings.MSSqlServerConnectionString))
                {
                    var comma = new SqlCommand($"drop database if exists {SolutionSettings.DatabaseName}", connection);

                    connection.Open();
                    comma.ExecuteNonQuery();
                    connection.Close();
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
    }
}