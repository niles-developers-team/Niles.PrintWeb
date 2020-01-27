using Microsoft.Extensions.Logging;
using Niles.PrintWeb.DataAccessObjects.Interfaces;

namespace Niles.PrintWeb.DataAccessObjects.SqlServer
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

        public IUserDao UserDao => new UserDao(_connectionString, _logger);
    }
}