using coop2._0.Entities;
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

        public UserService(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<ItemsModel<UserItemModel>> GetAll(int page)
        {
            IEnumerable<UserItemModel> users = await _userRepository.FindAll(page);
            int itemNum = await _userRepository.GetCount();
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

        public async Task<bool> Validate(List<string> users, int page)
        {
            bool temoin = true;
            foreach(string id in users)
            {
                var user = await _userRepository.GetUserById(id);
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
                var user = await _userRepository.GetUserById(id);
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
                var user = await _userRepository.GetUserById(id);
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