using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.Models.Entities;

namespace Niles.PrintWeb.Services
{
    public interface ITenantService
    {
        Task<Tenant> Create(Tenant model);
        Task<IEnumerable<Tenant>> Get(TenantGetOptions options);
        Task<Tenant> Update(Tenant model);
        Task Delete(IReadOnlyList<int> ids);
    }
}