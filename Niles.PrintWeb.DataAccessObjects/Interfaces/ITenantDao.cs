using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.DataAccessObjects.Interfaces
{
    public interface ITenantDao
    {
        Task Create(Tenant model);
        Task<IEnumerable<Tenant>> Get(TenantGetOptions options);
        Task Update(Tenant model);
        Task Delete(IReadOnlyList<int> ids);        
    }
}