using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task ConfirmUser(Guid code);
        Task<User> Create(UserAuthenticated model);
        Task<IEnumerable<User>> Get(UserGetOptions options);
        Task<UserAuthenticated> SignIn(UserGetOptions options);
        Task<User> Update(User model);
        Task<string> Validate(UserGetOptions options);
    }
}