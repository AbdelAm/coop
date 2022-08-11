using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IBankAccountRepository _bankRepository;
        private readonly IRequestRepository _requestRepository;

        public UserService(IUserRepository userRepository, IMailService mailService, IBankAccountRepository bankAccount, IRequestRepository requestRepository)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _bankRepository = bankAccount;
            _requestRepository = requestRepository;
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

        public async Task<Response> Validate(List<string> users)
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
                    if(!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("There is problem with sending Reset Password Mail, please verify your email");
                    }
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
            return new Response
            {
                Status = "Success",
                Message = "the user has been validated successfully"
            };
        }

        public async Task<Response> Reject(List<string> users)
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
                    if (!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("There is problem with sending Reset Password Mail, please verify your email");
                    }
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
            return new Response
            {
                Status = "Success",
                Message = "The user has been Rejected successfully"
            };
        }

        public async Task<Response> Delete(List<string> users)
        {
            bool temoin = true;
            foreach (string id in users)
            {
                var user = await _userRepository.SelectById(id);
                var bankAccounts = await _bankRepository.SelectByUser(id);
                if (bankAccounts != null)
                {
                    if(bankAccounts.Where(b => b.Status == Status.Approuved).Any())
                    {
                        throw new Exception("This user has an approuved bankaccount, and may has done some transactions with it, you need to verify his bank account situation before delete the user");
                    }
                }
                var requests = await _requestRepository.SelectByUser(id);
                foreach(var bankAccount in bankAccounts)
                {
                    await _bankRepository.Delete(bankAccount);
                }
                foreach (var request in requests)
                {
                    await _requestRepository.RemoveRequest(request);
                }
                
                var result = await _userRepository.DeleteUser(user);
                if (result.Succeeded) 
                {
                    string message = "Unfortunately, your account has been Deleted";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Deleted",
                        Body = user.Name + '-' + message,
                    };
                    if (!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("There is problem with sending Reset Password Mail, please verify your email");
                    }
                } else { temoin = false; }
            }
            if (!temoin)
            {
                throw new Exception("the user doesn't deleted, please try again later");
            }
            return new Response
            {
                Status = "Success",
                Message = "The user has been Deleted successfully"
            };
        }

        public async Task<Response> ChangeInfo(UserInfoModel model)
        {
            var user = await _userRepository.SelectById(model.Cif);
            user.Name = model.Name;
            user.SocialNumber = model.SocialNumber;
            user.PhoneNumber = model.Phone;
            
            var result = await _userRepository.UpdateUser(user);

            if(!result.Succeeded)
            {
                throw new Exception("An error occured on updating the user, please try again later");
            }
            return new Response
            {
                Status = "Success",
                Message = "The user information has been Changed successfully"
            };
        }

        public async Task<Response> ChangeEmail(EmailUpdateModel model)
        {
            var user = await _userRepository.SelectById(model.Cif);
            bool temoin = await _userRepository.EmailExists(model.NewEmail);
            if(temoin)
            {
                throw new Exception("This email is Already exists, please enter another valid email");
            }
            user.Email = model.NewEmail;
            user.EmailConfirmed = false;

            var result = await _userRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                throw new Exception("An error occured on updating the user, please try again later");
            }

            string token = await _userRepository.GenerateConfirmationToken(user);
            
            await _mailService.SendConfirmMail(user, token);

            return new Response
            {
                Status = "success",
                Message = "Email has been updated successfully, you should confirm your new email by clicking in link sent to your new email"
            };
        }

        public async Task<Response> ChangePassword(PasswordUpdateModel model)
        {
            Exception e = new Exception();
            var user = await _userRepository.SelectById(model.Cif);
            bool temoin = await _userRepository.CheckPassword(user, model.CurrentPassword);
            if (!temoin)
            {
                e.Data.Add("password_error", "The current password is not correct");
                throw e;
            }
            string token = await _userRepository.GenerateResetToken(user);
            IdentityResult res = await _userRepository.ResetPassword(user, token, model.NewPassword);
            if (!res.Succeeded)
            {
                e.Data.Add("new_password_error", "the password doesn't change, please try again later");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "Password has been changed, you can login with new Password"
            };
        }
    }
}