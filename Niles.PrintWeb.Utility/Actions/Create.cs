using System;
using CommandLine;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Shared;
using System.Data.SqlClient;

namespace Niles.PrintWeb.Utility.Actions
{
    [Verb("create", HelpText = "Create the DB")]
    class CreateOptions { }

    public class Create
    {
        public static int Run(ILogger logger)
        {
            try
            {
                logger.LogInformation($"Try to create \"{SolutionSettings.DatabaseName}\" database");

                using (var connection = new SqlConnection(SolutionSettings.MSSqlServerConnectionString))
                {
                    var comma = new SqlCommand($@"
                        create database {SolutionSettings.DatabaseName}
                    ", connection);

                    connection.Open();
                    comma.ExecuteNonQuery();
                    connection.Close();
                }

                logger.LogInformation($"{SolutionSettings.DatabaseName} database successfully created");
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