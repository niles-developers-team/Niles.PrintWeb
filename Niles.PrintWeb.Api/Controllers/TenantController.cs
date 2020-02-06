using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Services;

namespace Niles.PrintWeb.Api.Controllers
{
    [Route("api/tenant")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService ?? throw new ArgumentException(nameof(tenantService));
        }

        [Authorize(Roles = "Admin, Tenant")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]TenantGetOptions options) => Ok(await _tenantService.Get(options));

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Tenant model)
        {
            string message = await _tenantService.Validate(new TenantValidateOptions { Name = model.Name });
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new { message });

            var result = await _tenantService.Create(model);

            if (result == null)
                return BadRequest(new { message = "There was some errors with tenant creating" });

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Tenant")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]Tenant model)
        {
            string message = await _tenantService.Validate(new TenantValidateOptions { Id = model.Id, Name = model.Name });
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new { message });

            var result = await _tenantService.Update(model);

            if (result == null)
                return BadRequest(new { message = "There was some errors with tenant updating" });

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Tenant")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]IReadOnlyList<int> ids)
        {
            await _tenantService.Delete(ids);

            return Ok();
        }
    }
}