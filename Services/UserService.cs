﻿using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IBankAccountRepository _bankRepository;

        public UserService(IUserRepository userRepository, IMailService mailService, IBankAccountRepository bankAccount)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _bankRepository = bankAccount;
        }

        public async Task<ItemsModel<UserItemModel>> FindUsers(int page)
        {
            IEnumerable<UserItemModel> users = await _userRepository.SelectAll(page);
            int itemNum = await _userRepository.SelectCount();
            if (users == null)
            {
                throw new Exception("There is no users existe");
            }
            return new ItemsModel<UserItemModel>
            {
                Items = users,
                ItemsNumber = itemNum
            };
        }
        public async Task<UserItemModel> FindUser(string cif)
        {
            User user = await _userRepository.SelectById(cif);
            if (user == null)
            {
                throw new Exception("There is no user existe with these information");
            }
            return new UserItemModel(user);
        }

        public async Task<IEnumerable<UserItemModel>> FindBy(string value)
        {
            IEnumerable<UserItemModel> users = await _userRepository.SelectBy(value);
            if(users == null)
            {
                throw new Exception("There is no users existe with this value");
            }
            return users;
        }

        public async Task<bool> Validate(List<string> users, int page)
        {
            bool temoin = true;
            foreach(string id in users)
            {
                var user = await _userRepository.SelectById(id);
                user.Status = Status.Approuved;
                var result = await _userRepository.UpdateUser(user);
                if (result.Succeeded) 
                {
                    string message = "Congratulation your account has been approuved successfully";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Approuved",
                        Body = user.Name + '-' + message,
                    };
                    await _mailService.SendValidationMail(mailModel);
                    BankAccount account = new BankAccount()
                    {
                        AccountNumber = Guid.NewGuid().ToString("D"),
                        Balance = 350.0,
                        DateCreated = DateTime.Now,
                        UserId = user.Id
                    };
                    await _bankRepository.InsertBankAccount(account);
                } else
                {
                    temoin = false;
                }
            }
            if(!temoin)
            {
                throw new Exception("some users doesn't approuved, please try again later");
            }
            return temoin;
        }

        public async Task<bool> Reject(List<string> users, int page)
        {
            bool temoin = true;
            foreach (string id in users)
            {
                var user = await _userRepository.SelectById(id);
                user.Status = Status.Rejected;
                var result = await _userRepository.UpdateUser(user);
                if (result.Succeeded)
                {
                    string message = "Unfortunately, your account has been rejected";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Rejected",
                        Body = user.Name + '-' + message,
                    };
                    await _mailService.SendValidationMail(mailModel);
                }
                else
                {
                    temoin = false;
                }
            }
            if (!temoin)
            {
                throw new Exception("some users doesn't rejected, please try again later");
            }
            return temoin;
        }

        public async Task<bool> Delete(List<string> users, int page)
        {
            bool temoin = true;
            foreach (string id in users)
            {
                var user = await _userRepository.SelectById(id);
                var result = await _userRepository.DeleteUser(user);
                if (!result.Succeeded) temoin = false;
            }
            if (!temoin)
            {
                throw new Exception("some users doesn't deleted, please try again later");
            }
            return temoin;
        }
    }
}