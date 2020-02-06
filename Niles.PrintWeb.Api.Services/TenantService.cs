using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Services;

namespace Niles.PrintWeb.Api.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantDao _dao;

        public TenantService(ITenantDao dao)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
        }

        public async Task<Tenant> Create(Tenant model)
        {
            await _dao.Create(model);

            return model;
        }

        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        public async Task<IEnumerable<Tenant>> Get(TenantGetOptions options)
        {
            var tenants = await _dao.Get(options);

            return tenants;
        }

        public async Task<Tenant> Update(Tenant model)
        {
            await _dao.Update(model);

            return model;
        }
    }
}