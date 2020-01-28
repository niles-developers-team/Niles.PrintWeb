using System;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.DataAccessObjects.SqlServer
{
    public class DaoFactory : IDaoFactory
    {
        private readonly DatabaseConnectionSettings _settings;
        private readonly ILogger _logger;

        public DaoFactory(DatabaseConnectionSettings settings, ILogger logger)
        {
            _settings = settings ?? throw new ArgumentException(nameof(settings));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public IUserDao UserDao => new UserDao(_settings, _logger);
    }
}