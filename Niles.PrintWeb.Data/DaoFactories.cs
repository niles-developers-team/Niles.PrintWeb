using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Data.Enumerations;
using Niles.PrintWeb.Data.Interfaces;

namespace Niles.PrintWeb.Data
{    
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DataProvider provider, string connectionString, ILogger logger)
        {
            switch (provider)
            {
                case DataProvider.MSSql:
                    return new DataAccessObjects.MSSql.DaoFactory(connectionString, logger);
                default:
                    return new DataAccessObjects.MSSql.DaoFactory(connectionString, logger);
            }
        }
    }
}