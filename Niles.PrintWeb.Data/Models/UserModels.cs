using System;
using System.Collections.Generic;

namespace Niles.PrintWeb.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdate { get; set; }

        public bool Confirmed { get; set; }
    }

    public class UserAuthenticated : User
    {
        public Guid Code { get; set; }

        public bool AuthenticateIsUsed { get; set; }

        public string Token { get; set; }
    }

    public class UserGetOptions
    {
        public int? Id { get; set; }

        public IReadOnlyList<int> Ids { get; set; }

        public bool OnlyConfirmed { get; set; } = true;
        
        public bool RememberMe { get; set; }

        public string Search { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}