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
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(DatabaseConnectionSettings settings, ILogger _logger) : base(settings, _logger) { }

        public async Task Create(User model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create user query");
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                    insert into [User] (UserName, Password, FirstName, LastName, Email, ConfirmCode, Role)
                    values (@UserName, pwdencrypt(@Password), @FirstName, @LastName, @Email, @ConfirmCode, @Role);
                    select SCOPE_IDENTITY();
                ", model);
                _logger.LogInformation("Sql create user query successfully executed");
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
                        ConfirmCode,
                        Role,
                        DateCreated,
                        DateUpdated
                    from [User]
                ");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Id = @Id");

                if (options.Role.HasValue)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Role = @Role");

                if (options.Ids != null)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} id = any(@Ids)");

                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                    sql.AppendLine($@"
                        {(conditionIndex++ == 0 ? "where" : "and")} FirstName like lower(@NormalizedSearch)
                        or LastName like lower(@NormalizedSearch)
                        or Email like lower(@NormalizedSearch)
                        or UserName like lower(@NormalizedSearch)
                    ");

                if (options.OnlyConfirmed)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} ConfirmCode is null");

                if (!string.IsNullOrEmpty(options.UserName))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} UserName = @UserName");

                if (!string.IsNullOrEmpty(options.Email))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Email = @Email");

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

        public async Task<IEnumerable<User>> Get(UserAuthorizeOptions options)
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
                        ConfirmCode,
                        Role
                    from [User]
                ");

                int conditionIndex = 0;

                if (!string.IsNullOrEmpty(options.UserNameOrEmail))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (UserName = @UserNameOrEmail or Email = @UserNameOrEmail)");

                if (!string.IsNullOrEmpty(options.Password))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (pwdcompare(@Password, Password) = 1)");

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

        public async Task<IEnumerable<User>> Get(UserValidateOptions options)
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
                        ConfirmCode
                    from [User]
                ");

                int conditionIndex = 0;

                if (options.Id.HasValue)
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Id <> @Id");

                if (!string.IsNullOrEmpty(options.UserName))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} UserName = @UserName");

                if (!string.IsNullOrEmpty(options.Email))
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} Email = @Email");

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

        public async Task Update(User model)
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
                    from [User]
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

        public async Task Delete(IReadOnlyList<int> ids)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql delete users query");
                await ExecuteAsync(@"
                    delete from [User]
                    where id in @ids
                ", new { ids });
                _logger.LogInformation("Sql delete users query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Confirm(Guid code)
        {
            try
            {
                _logger.LogInformation("Try to confirm user.");

                int? userId = await QueryFirstOrDefaultAsync<int?>(@"
                        select UserId
                        from [User]
                        where ConfirmCode = @code
                    ", new { code });

                if (!userId.HasValue)
                {
                    _logger.LogInformation("User have been already confirmed or not found.");
                }
                _logger.LogInformation("User successfully found.");

                await ExecuteAsync(@"
                            update
                                ConfirmCode = null
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
    }
}