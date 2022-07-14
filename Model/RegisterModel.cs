using coop2._0.Entity;
using System;

namespace coop2._0.Model
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
        public string CifNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsConfirmed { get; set; }
        public Status Status { get; set; }
    }
}
