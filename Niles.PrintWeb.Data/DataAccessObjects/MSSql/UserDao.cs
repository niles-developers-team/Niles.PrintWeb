using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Data.Interfaces;
using Niles.PrintWeb.Data.Models;

namespace Niles.PrintWeb.Data.DataAccessObjects
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(string connectionString, ILogger _logger) : base(connectionString, _logger) { }

        public async Task ConfirmUser(Guid userCode)
        {
            try
            {
                _logger.LogInformation("Try to confirm user.");

                int? userId = await QueryFirstOrDefaultAsync<int?>(@"
                        select UserId
                        from NotConfirmedUser
                        where ConfirmCode = @userCode
                    ", new { userCode });

                if (!userId.HasValue)
                {
                    _logger.LogInformation("User have been already confirmed or not found.");
                }
                _logger.LogInformation("User successfully found.");

                await ExecuteAsync(@"
                            delete from NotConfirmedUser
                            where UserId = @userId
                        ", new { userId });
                _logger.LogInformation("User successfully confirmed.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Create(UserAuthenticate model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create user query");
                model.Code = Guid.NewGuid();
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                    begin transaction
                        insert into User (UserName, PasswordHash, FirstName, LastName, Email)
                        values (@UserName, crypt(@Password, gen_salt('bf')), @FirstName, @LastName, @Email)
                        returning Id;

                        insert into not_confirmed_users (UserId, ConfirmCode)
                        values (@Id, @Code);
                    commit;
                ", model);
                _logger.LogInformation("Sql create user query successfully executed");
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
                _logger.LogInformation("Trying to execute sql delete users query");
                await ExecuteAsync(@"
                    delete from User
                    where id = any(@ids)
                ", new { ids });
                _logger.LogInformation("Sql delete users query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<User>> Get(UserGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get users sql query");

                sql.AppendLine(@"
                    select 
                        Id,
                        UserName,
                        FirstName,
                        LastName,
                        Email,
                        case ncu.UserId
                            when null then true
                            else false
                        end as Confirmed
                    from User
                    left join NotConfirmedUser ncu on Id = ncu.UserId");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Id = @Id");
                }
                if (options.Ids != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} id = any(@Ids)");
                }
                if (options.Search != null)
                {
                    sql.AppendLine($@"
                        {(conditionIndex++ == 0 ? "where" : "and")} FirstName like '%@Search%'
                        or LastName like '%Search%'
                        or Email like '%Search%'
                        or UserName like '%Search%'
                    ");
                }
                if (options.OnlyConfirmed)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} ncu.Id is null");
                }
                if (!string.IsNullOrEmpty(options.UserName) && !string.IsNullOrEmpty(options.Password))
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} UserName = @UserName and PasswordHash = crypt(@Password, PasswordHash)");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get users query");
                var result = await QueryAsync<User>(sql.ToString(), options);
                _logger.LogInformation("Sql get users query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(UserAuthenticate model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update user query");
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                    update
                        UserName = @UserName, 
                        FirstName = @FirstName, 
                        LastName = @LastName, 
                        Email = @Email
                    from User
                    where Id = @Id
                ", model);
                _logger.LogInformation("Sql update user query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}