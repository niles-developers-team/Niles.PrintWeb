using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Api.Services.Interfaces;

namespace Niles.PrintWeb.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]UserGetOptions options)
        {
            return Ok(await _userService.Get(options));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]User model)
        {
            string message = await _userService.Validate(new UserValidateOptions { UserName = model.UserName, Email = model.Email });
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new { message });

            var result = await _userService.Create(model);

            if (result == null)
                return BadRequest(new { message = "There was some errors with user creating" });

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]User model)
        {
            string message = await _userService.Validate(new UserValidateOptions { Id = model.Id, UserName = model.UserName, Email = model.Email });
            if (!string.IsNullOrEmpty(message))
                return BadRequest(new { message });

            var result = await _userService.Update(model);

            if (result == null)
                return BadRequest(new { message = "There was some errors with user updating" });

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateUser([FromQuery]UserValidateOptions options)
        {
            var result = await _userService.Validate(options);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody]Guid code)
        {
            await _userService.Confirm(code);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody]UserAuthorizeOptions options)
        {
            var result = await _userService.SignIn(options);

            if (result == null)
                return BadRequest(new { message = "User name or password is incorrect" });

            return Ok(result);
        }
    }
}