using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Data.Interfaces;

namespace Niles.PrintWeb.Data.DataAccessObjects.MSSql
{
    public class DaoFactory : IDaoFactory
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DaoFactory(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
    }
}