using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace coop2._0.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SocialNumber { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public DateTime DateCreated { get; set; }
        [DefaultValue(false)] public bool IsAdmin { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DefaultValue(Status.Progress)]
        public Status Status { get; set; }
    }
}