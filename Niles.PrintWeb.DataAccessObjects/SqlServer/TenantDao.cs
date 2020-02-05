using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.DataAccessObjects.SqlServer
{
    public class TenantDao : BaseDao, ITenantDao
    {
        public TenantDao(DatabaseConnectionSettings settings, ILogger logger) : base(settings, logger) { }

        public async Task Create(Tenant model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create tenant query");
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                    insert into [Tenant] (Name, SubscriptionId, SubscribeDate)
                    values (@Name, @SubscriptionId, @SubscribeDate);
                    select SCOPE_IDENTITY();
                ", model);
                _logger.LogInformation("Sql create tenant query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Delete(IReadOnlyList<int> ids)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql delete tenants query");
                await ExecuteAsync(@"
                    delete from [Tenant]
                    where Id = any(@ids)
                ", new { ids });
                _logger.LogInformation("Sql delete tenants query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<Tenant>> Get(TenantGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get tenants sql query");

                sql.AppendLine(@"
                    select 
                        Id,
                        Name,
                        DateCreated,
                        SubscriptionId,
                        SubscribeDate
                    from [Tenant]
                ");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Id = @Id");

                if (options.Ids != null)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} id = any(@Ids)");

                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                    sql.AppendLine($@"
                        {(conditionIndex++ == 0 ? "where" : "and")} FirstName like lower(@NormalizedSearch)
                        or LastName like lower(@NormalizedSearch)
                        or Email like lower(@NormalizedSearch)
                        or UserName like lower(@NormalizedSearch)
                    ");
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get tenants query");
                var result = await QueryAsync<Tenant>(sql.ToString(), options);
                _logger.LogInformation("Sql get tenants query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(Tenant model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update tenant query");
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                    update
                        Name = @Name, 
                        SubscriptionId = @SubscriptionId, 
                        SubscribeDate = @SubscribeDate
                    from [Tenant]
                    where Id = @Id
                ", model);
                _logger.LogInformation("Sql update tenant query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}