﻿using coop2._0.Entities;

namespace coop2._0.Model
{
    public class UserItemModel
    {
        public string Cif { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long SocialNumber { get; set; }
        public bool IsConfirmed { get; set; }
        public Status Status { get; set; }


        public UserItemModel()
        {
        }

        public UserItemModel(User u)
        {
            Cif = u.Id;
            Name = u.Name;
            Email = u.Email;
            Phone = u.PhoneNumber;
            SocialNumber = u.SocialNumber;
            IsConfirmed = u.EmailConfirmed;
            Status = u.Status;
        }
    }
}