using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.DataAccessObjects
{
    public abstract class BaseDao
    {
        protected readonly DatabaseConnectionSettings _settings;
        protected readonly ILogger _logger;
        protected BaseDao(DatabaseConnectionSettings settings, ILogger logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return connection.Query<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return connection.QueryFirst<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return connection.QueryFirstOrDefault<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryAsync<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<T> QueryFirstAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryFirstAsync<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected T QuerySingle<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return connection.QuerySingle<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected T QuerySingleOrDefault<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return connection.QuerySingleOrDefault<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleAsync<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }

        protected void Execute(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    connection.Execute(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
        protected async Task ExecuteAsync(string sql, object parameters = null)
        {
            using (IDbConnection connection = DatabaseConnectionSettings.CreateDatabaseConnection(_settings))
            {
                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(sql, parameters);
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                    throw exc;
                }
            }
        }
    }
}