using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Data.Interfaces;
using Niles.PrintWeb.Data.Models;
using Niles.PrintWeb.Shared;
using Niles.PrintWeb.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace Niles.PrintWeb.Services
{
    public class UserService
    {
        private readonly IUserDao _dao;

        private readonly EmailService _emailService;
        private readonly ApplicationSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public UserService(
            IUserDao dao,
            EmailService emailService,
            ApplicationSettings settings,
            IHttpContextAccessor httpContextAccessor,
            ILogger logger
        )
        {
            _dao = dao ?? throw new ArgumentNullException(nameof(dao));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ConfirmUser(Guid code)
        {
            await _dao.ConfirmUser(code);
        }

        public async Task<UserAuthenticate> Create(UserAuthenticate model)
        {
            await _dao.Create(model);

            string url = string.Empty;
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Origin"))
            {
                var origin = _httpContextAccessor.HttpContext.Request.Headers["Origin"];
                url = $"{origin}/confirm-email/{model.Code}";
            }
            else
            {
                Exception exception = new Exception("Cannot find origin resource");
                throw exception;
            }

            string subject = "Email Confirmation";
            string message = _emailService.CreateVerifyMailMessageContent(subject, "We just want to confirm that was you", url);
            await _emailService.SendEmailAsync(model.Email, subject, message);

            model.Password = null;

            return model;
        }

        public async Task<IEnumerable<User>> Get(UserGetOptions options)
        {
            return await _dao.Get(options);
        }

        public async Task<UserAuthenticate> SignIn(UserGetOptions options)
        {
            var users = await _dao.Get(options);
            var user = users.FirstOrDefault() as UserAuthenticate;
            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            if (options.RememberMe)
            {
                if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("graph-master-token"))
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete("graph-master-token");
                }
                _httpContextAccessor.HttpContext.Response.Cookies.Append("graph-master-token", user.Token);
            }

            user.Password = null;

            return user;
        }

        public async Task<UserAuthenticate> Update(UserAuthenticate model)
        {
            await _dao.Update(model);

            return model;
        }

        public async Task<string> ValidateUserName(string userName)
        {
            try
            {
                _logger.LogInformation("Start user name validating.");

                string result = ValidationUtilities.ValidateUserName(userName);
                if (!string.IsNullOrEmpty(result))
                    return result;

                var users = await _dao.Get(new UserGetOptions { UserName = userName });
                if (users != null || users.Count() > 0)
                {
                    string message = "User with same user name have been already created. Please try another or try to sign in.";
                    _logger.LogInformation(message);
                    return message;
                }

                _logger.LogInformation("User name successfuly validated.");
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<string> ValidateEmail(string email)
        {
            try
            {
                _logger.LogInformation("Start email validating.");

                string result = ValidationUtilities.ValidateEmail(email);
                if (!string.IsNullOrEmpty(result))
                    return result;

                var users = await _dao.Get(new UserGetOptions { Email = email });
                if (users != null || users.Count() > 0)
                {
                    string message = "User with same email have been already created. Please try another or try to sign in.";
                    _logger.LogInformation(message);
                    return message;
                }

                _logger.LogInformation("Email successfuly validated.");
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}