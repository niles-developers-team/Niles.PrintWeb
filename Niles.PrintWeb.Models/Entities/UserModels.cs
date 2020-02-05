using System;
using System.Collections.Generic;
using Niles.PrintWeb.Models.Enumerations;

namespace Niles.PrintWeb.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Roles Role { get; set; }

        public Guid? ConfirmCode { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdate { get; set; }

        public bool Confirmed => !ConfirmCode.HasValue;
    }

    public class AuthenticatedUser : User
    {
        public string Token { get; set; }
    }

    public class UserGetOptions : BaseGetOptions
    {
        public Roles? Role { get; set; }

        public bool OnlyConfirmed { get; set; } = true;

        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        
        public string Search { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }

    public class UserAuthorizeOptions
    {
        public string UserNameOrEmail { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class UserValidateOptions
    {
        public int? Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}