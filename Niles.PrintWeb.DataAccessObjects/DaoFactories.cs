using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings.Enumerations;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.DataAccessObjects
{    
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DatabaseConnectionSettings settings, ILogger logger)
        {
            switch (settings.Provider)
            {
                case DatabaseProvider.SqlServer:
                    return new DataAccessObjects.SqlServer.DaoFactory(settings, logger);
                default:
                    return new DataAccessObjects.SqlServer.DaoFactory(settings, logger);
            }
        }
    }
}