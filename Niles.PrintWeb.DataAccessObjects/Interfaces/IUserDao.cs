using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.DataAccessObjects.Interfaces
{
    public interface IUserDao
    {
        Task Create(User model);
        Task<IEnumerable<User>> Get(UserGetOptions options);
        Task<IEnumerable<User>> Get(UserAuthorizeOptions options);
        Task<IEnumerable<User>> Get(UserValidateOptions options);
        Task Update(User model);
        Task Delete(IReadOnlyList<int> ids);

        Task Confirm(Guid code);
    }
}