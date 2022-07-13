using Microsoft.AspNetCore.Identity;
using System;

namespace coop2._0.Entity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
        public string CifNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAdmin { get; set; }
        public bool isValid { get; set; }
    }
}
