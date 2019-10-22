using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Data.Models;

namespace Niles.PrintWeb.Data.Interfaces
{
    public interface IUserDao
    {        
        Task<IEnumerable<User>> Get(UserGetOptions options);
        Task Create(UserAuthenticate model);
        Task Update(UserAuthenticate model);
        Task Delete(IReadOnlyList<int> ids);
    }
}