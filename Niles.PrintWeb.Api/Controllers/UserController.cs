using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Niles.PrintWeb.Data.Models;
using Niles.PrintWeb.Services;

namespace Niles.PrintWeb.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }        

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserGetOptions options)
        {
            var result = await _userService.Get(options);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery]UserGetOptions options)
        {
            var users = await _userService.Get(options);
            var user = users.FirstOrDefault();
            if(user == null)
            {
                return NotFound("Cannot find user");
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserAuthenticate model)
        {
            var result = await _userService.Create(model);

            if(result == null) {
                return BadRequest(new { message = "There was some errors with user creating" });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UserAuthenticate model)
        {
            var result = await _userService.Update(model);

            if(result == null) {
                return BadRequest(new { message = "There was some errors with user updating" });
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("validate/username")]
        public async Task<IActionResult> ValidateUserName([FromQuery]string userName)
        {
            var result = await _userService.ValidateUserName(userName);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("validate/email")]
        public async Task<IActionResult> ValidateEmail([FromQuery] string email)
        {
            var result = await _userService.ValidateEmail(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmUser([FromBody]Guid code) 
        {
            await _userService.ConfirmUser(code);

            return Ok();
        }
        
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody]UserGetOptions options)
        {
            var result = await _userService.SignIn(options);

            if (result == null)
                return Ok(new { message = "User name or password is incorrect" });

            return Ok(result);
        }
    }
}