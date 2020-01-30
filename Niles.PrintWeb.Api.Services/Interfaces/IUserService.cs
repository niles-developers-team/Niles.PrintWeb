using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(User model);
        Task<IEnumerable<User>> Get(UserGetOptions options);
        Task<User> Update(User model);
        Task Delete(IReadOnlyList<int> ids);
        Task Confirm(Guid code);
        Task<AuthenticatedUser> SignIn(UserAuthorizeOptions options);
        Task<string> Validate(UserValidateOptions options);
    }
}