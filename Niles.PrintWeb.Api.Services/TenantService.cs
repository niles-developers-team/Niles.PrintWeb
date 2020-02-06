using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Services;
using Niles.PrintWeb.Shared;

namespace Niles.PrintWeb.Api.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantDao _dao;

        private readonly ILogger _logger;

        public TenantService(ITenantDao dao, ILogger logger)
        {
            _dao = dao ?? throw new ArgumentException(nameof(dao));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
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

        public async Task<string> Validate(TenantValidateOptions options)
        {
            try
            {
                _logger.LogInformation("Start tenant name validating.");

                string result = ValidateTenantName(options.Name);
                if (!string.IsNullOrEmpty(result))
                    return result;

                var tenants = await _dao.Get(options);
                if (tenants != null && tenants.Count() > 0)
                {
                    string message = "Tenant with same name or email have been already created. Please try another or try to sign in.";
                    _logger.LogInformation(message);
                    return message;
                }

                _logger.LogInformation("Tenant successfuly validated.");
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        private string ValidateTenantName(string name)
        {
            if (!ValidationUtilities.NotEmptyRule(name))
                return "Tenant name should not be empty.";

            if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(name))
                return "Tenant name must contains only letters, numbers and underscores.";

            return string.Empty;
        }
    }
}