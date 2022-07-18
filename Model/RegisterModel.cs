using coop2._0.Entities;
using System;

namespace coop2._0.Model
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}