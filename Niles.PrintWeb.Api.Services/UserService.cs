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
using Niles.PrintWeb.Models.Settings;
using Niles.PrintWeb.Services;

namespace Niles.PrintWeb.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDao _dao;

        private readonly Appsettings _settings;

        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public UserService(
            IUserDao dao,
            IEmailService emailService,
            Appsettings settings,
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

        public async Task<User> Create(User model)
        {
            model.ConfirmCode = Guid.NewGuid();
            await _dao.Create(model);

            string url = string.Empty;
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Origin"))
            {
                var origin = _httpContextAccessor.HttpContext.Request.Headers["Origin"];
                url = $"{origin}/confirm-email/{model.ConfirmCode}";
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

        public async Task<IEnumerable<User>> Get(UserGetOptions options) => await _dao.Get(options);

        public async Task<User> Update(User model)
        {
            await _dao.Update(model);

            return model;
        }

        public async Task Delete(IReadOnlyList<int> ids) => await _dao.Delete(ids);

        public async Task Confirm(Guid code) => await _dao.Confirm(code);

        public async Task<AuthenticatedUser> SignIn(UserAuthorizeOptions options)
        {
            var users = await _dao.Get(options);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var authenticatedUser = new AuthenticatedUser {
                ConfirmCode = user.ConfirmCode,
                DateCreated = user.DateCreated,
                DateUpdate = user.DateUpdate,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Role = user.Role,
                Token = null,
                UserName = user.UserName                
            };

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.GetDescription())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _settings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            authenticatedUser.Token = tokenHandler.WriteToken(token);

            if (options.RememberMe)
            {
                if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("printweb-token"))
                {
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete("printweb-token");
                }
                _httpContextAccessor.HttpContext.Response.Cookies.Append("printweb-token", authenticatedUser.Token);
            }

            return authenticatedUser;
        }

        public async Task<string> Validate(UserValidateOptions options)
        {
            try
            {
                _logger.LogInformation("Start user name and email validating.");

                string result = ValidateUserName(options.UserName);
                if (!string.IsNullOrEmpty(result))
                    return result;

                result = ValidatePassword(options.Password);
                if (!string.IsNullOrEmpty(result))
                    return result;

                result = ValidateEmail(options.Email);
                if (!string.IsNullOrEmpty(result))
                    return result;

                var users = await _dao.Get(options);
                if (users != null && users.Count() > 0)
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

        private string ValidateUserName(string userName)
        {
            if (!ValidationUtilities.NotEmptyRule(userName))
                return "User name should not be empty.";

            int minLength = 5;
            if (!ValidationUtilities.MoreThanValueLengthRule(userName, minLength))
                return $"User name length length should be more than {minLength}."; ;

            if (!ValidationUtilities.OnlyLettersNumbersAndUnderscorcesRule(userName))
                return "User name must contains only letters, numbers and underscores.";

            return string.Empty;
        }

        private string ValidatePassword(string password)
        {
            if (!ValidationUtilities.NotEmptyRule(password))
                return "Password should not be empty.";

            int minLength = 6;
            if (!ValidationUtilities.MoreThanValueLengthRule(password, minLength))
                return $"Password length should be more than {minLength}.";

            return string.Empty;
        }

        private string ValidateEmail(string email)
        {
            if (!ValidationUtilities.NotEmptyRule(email))
                return "Email should not be empty";

            if (!ValidationUtilities.CheckEmailFormat(email))
                return "Email is not valid";

            return string.Empty;
        }
    }
}