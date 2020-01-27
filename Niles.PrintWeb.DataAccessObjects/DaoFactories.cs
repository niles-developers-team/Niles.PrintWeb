using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Enumerations;
using Niles.PrintWeb.DataAccessObjects.Interfaces;

namespace Niles.PrintWeb.DataAccessObjects
{    
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DataProvider provider, string connectionString, ILogger logger)
        {
            switch (provider)
            {
                case DataProvider.MSSql:
                    return new DataAccessObjects.SqlServer.DaoFactory(connectionString, logger);
                default:
                    return new DataAccessObjects.SqlServer.DaoFactory(connectionString, logger);
            }
        }
    }
}