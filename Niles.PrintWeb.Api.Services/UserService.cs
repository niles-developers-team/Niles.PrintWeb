using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.DataAccessObjects.Interfaces;
using Niles.PrintWeb.Models.Entities;
using Niles.PrintWeb.Shared;
using Microsoft.IdentityModel.Tokens;

namespace Niles.PrintWeb.Api.Services
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

        public async Task<User> Create(UserAuthenticated model)
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

        public async Task<UserAuthenticated> SignIn(UserGetOptions options)
        {
            var users = await _dao.Get(options);
            var user = users.FirstOrDefault() as UserAuthenticated;
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

        public async Task<User> Update(User model)
        {
            await _dao.Update(model);

            return model;
        }

        public async Task<string> Validate(UserGetOptions options)
        {
            try
            {
                _logger.LogInformation("Start user name and email validating.");

                string result = ValidateUserName(options.UserName);
                if (!string.IsNullOrEmpty(result))
                    return result;

                result = ValidateEmail(options.Email);
                if (!string.IsNullOrEmpty(result))
                    return result;

                var users = await _dao.Get(options);
                if (users != null || users.Count() > 0)
                {
                    string message = "User with same user name or email have been already created. Please try another or try to sign in.";
                    _logger.LogInformation(message);
                    return message;
                }

                _logger.LogInformation("User name and email successfuly validated.");
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public string ValidateUserName(string userName)
        {
            if (!ValidationUtilities.NotEmptyRule(userName))
                return "User name should not be empty.";

            if (!ValidationUtilities.MoreThanValueLengthRule(userName, 5))
                return "User name is to short.";

            if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(userName))
                return "User name must contains only letters, numbers and underscores.";

            return string.Empty;
        }

        public string ValidateEmail(string email)
        {
            if (ValidationUtilities.NotEmptyRule(email))
                return "Email should not be empty";

            if(ValidationUtilities.CheckEmailFormat(email))
                return "Email is not valid";

            return string.Empty;
        }
    }
}