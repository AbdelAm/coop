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
                throw new Exception("No existe usuarios con estas informaciones.");
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
                throw new Exception("No existe un usuario con estas informaciones.");
            }
            return new UserItemModel(user);
        }

        public async Task<IEnumerable<UserItemModel>> FindBy(string value)
        {
            IEnumerable<UserItemModel> users = await _userRepository.SelectBy(value);
            if(users == null)
            {
                throw new Exception("No existe usuarios con este valor");
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
                    string message = "Felicitaciones que su cuenta se ha aprobado con éxito";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Approuved",
                        Body = user.Name + '-' + message,
                    };
                    if(!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("Hay un problema con el envío del correo de contraseña de reinicio, verifique su correo electrónico");
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
                throw new Exception("Algunos usuarios no han sido aprendidos, inténtelo de nuevo más tarde");
            }
            return new Response
            {
                Status = "Success",
                Message = "El usuario ha sido validado con éxito"
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
                    string message = "Desafortunadamente, su cuenta ha sido rechazada";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Rejected",
                        Body = user.Name + '-' + message,
                    };
                    if (!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("Hay un problema con el envío del correo de contraseña de reinicio, verifique su correo electrónico");
                    }
                }
                else
                {
                    temoin = false;
                }
            }
            if (!temoin)
            {
                throw new Exception("Algunos usuarios no han sido rechazados, inténtelo de nuevo más tarde");
            }
            return new Response
            {
                Status = "Success",
                Message = "El usuario ha sido rechazado con éxito"
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
                        throw new Exception("Este usuario tiene un BankAccount apropiado, y May ha realizado algunas transacciones con él, debe verificar su situación de cuenta bancaria antes de eliminar al usuario");
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
                    string message = "Desafortunadamente, su cuenta ha sido eliminada";
                    MailModel mailModel = new MailModel()
                    {
                        Email = user.Email,
                        Subject = "User Deleted",
                        Body = user.Name + '-' + message,
                    };
                    if (!await _mailService.SendValidationMail(mailModel))
                    {
                        throw new Exception("Hay un problema con el envío del correo de contraseña de reinicio, verifique su correo electrónico");
                    }
                } else { temoin = false; }
            }
            if (!temoin)
            {
                throw new Exception("El usuario no ha sido eliminado, inténtelo de nuevo más tarde");
            }
            return new Response
            {
                Status = "Success",
                Message = "El usuario ha sido eliminado con éxito"
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
                throw new Exception("Se produjo un error al actualizar al usuario, intente nuevamente más tarde");
            }
            return new Response
            {
                Status = "Success",
                Message = "La información del usuario se ha cambiado con éxito"
            };
        }

        public async Task<Response> ChangeEmail(EmailUpdateModel model)
        {
            var user = await _userRepository.SelectById(model.Cif);
            bool temoin = await _userRepository.EmailExists(model.NewEmail);
            if(temoin)
            {
                throw new Exception("Este correo electrónico ya existe, ingrese otro correo electrónico válido");
            }
            user.Email = model.NewEmail;
            user.EmailConfirmed = false;

            var result = await _userRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                throw new Exception("Se produjo un error al actualizar al usuario, intente nuevamente más tarde");
            }

            string token = await _userRepository.GenerateConfirmationToken(user);
            
            await _mailService.SendConfirmMail(user, token);

            return new Response
            {
                Status = "success",
                Message = "El correo electrónico se ha actualizado correctamente, debe confirmar su nuevo correo electrónico haciendo clic en el enlace enviado a su nuevo correo electrónico"
            };
        }

        public async Task<Response> ChangePassword(PasswordUpdateModel model)
        {
            Exception e = new Exception();
            var user = await _userRepository.SelectById(model.Cif);
            bool temoin = await _userRepository.CheckPassword(user, model.CurrentPassword);
            if (!temoin)
            {
                e.Data.Add("password_error", "La contraseña actual no es correcta");
                throw e;
            }
            string token = await _userRepository.GenerateResetToken(user);
            IdentityResult res = await _userRepository.ResetPassword(user, token, model.NewPassword);
            if (!res.Succeeded)
            {
                e.Data.Add("new_password_error", "La contraseña no cambia, intente nuevamente más tarde");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "Se ha cambiado la contraseña, puede iniciar sesión con una nueva contraseña"
            };
        }
    }
}