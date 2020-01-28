using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.DataAccessObjects.Interfaces
{
    public interface IUserDao
    {
        Task<IEnumerable<User>> Get(UserGetOptions options);
        Task Create(UserAuthenticated model);
        Task Update(User model);
        Task Delete(IReadOnlyList<int> ids);

        Task ConfirmUser(Guid userCode);
    }
}