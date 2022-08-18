using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IBankAccountRepository _bankRepository;

        public AuthentificationService(IUserRepository userRepository, IJwtService jwtService,
            IConfiguration configuration, IMailService mailService, IBankAccountRepository bankRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _mailService = mailService;
            _bankRepository = bankRepository;
        }

        public async Task<Response> Register(RegisterModel model)
        {
            Exception e = new();
            var u = await _userRepository.SelectByEmail(model.Email);
            if (u != null)
            {
                e.Data.Add("email_error", "El correo electrónico ya existe");
                throw e;
            }

            u = await _userRepository.SelectBySocialNumber(model.SocialNumber);
            if (u != null)
            {
                e.Data.Add("socialNumber_error", "El número social ya existe");
                throw e;
            }

            User user = new()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = Regex.Replace(model.Name.ToLower(), @"\s+", ""),
                PhoneNumber = model.Phone,
                SocialNumber = model.SocialNumber,
                DateCreated = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userRepository.InsertUser(user, model.Password);
            if (result == null)
            {
                e.Data.Add("user", "Hay un problema al registrar el usuario, intente nuevamente");
                throw e;
            }

            string token = await _userRepository.GenerateConfirmationToken(user);

            if (!await _mailService.SendConfirmMail(user, token))
            {
                e.Data.Add("user", "Hay un problema al enviar correo de confirmación, verifique su correo electrónico");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "Para completar su registro, se ha enviado un correo electrónico para confirmar su registro"
            };
        }

        public async Task<TokenModel> Login(LoginModel model)
        {
            var user = await _userRepository.SelectUser(model);

            if (user is null)
                throw new Exception("El usuario no existe");
            if (user is not { EmailConfirmed: true })
                throw new Exception("El usuario aún no ha sido confirmado");
            if (user is not { Status: Status.Approuved })
                throw new Exception("El usuario aún no ha sido aprobado");

            return await _jwtService.GenerateJwtToken(user);
        }

        public async Task<Response> ConfirmUser(string param)
        {
            var values = param.Split('-');
            var user = await _userRepository.SelectByEmail(values[1]);
            if (user is not { EmailConfirmed: false })
            {
                throw new Exception("El usuario ya está confirmado");
            }

            string token = values[0].Replace(' ', '+');
            IdentityResult res = await _userRepository.ConfirmEmail(user, token);
            if (!res.Succeeded)
            {
                throw new Exception("El correo electrónico no ha sido confirmado, intente nuevamente");
            }

            return new Response
            {
                Status = "Success",
                Message = "El usuario se confirma con éxito"
            };
        }

        public async Task<Response> ForgetPassword(ForgetPasswordModel model)
        {
            var user = await _userRepository.SelectByEmail(model.Email);
            Exception e = new();
            if (user == null)
            {
                e.Data.Add("email_error", "El correo electrónico no existe, verifique su información");
                throw e;
            }

            string token = await _userRepository.GenerateResetToken(user);
            token = System.Web.HttpUtility.UrlEncode(token);

            if (!await _mailService.SendForgetMail(user, token))
            {
                e.Data.Add("email_error",
                    "Hay un problema con el envío del correo de contraseña de reinicio, verifique su correo electrónico");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message =
                    "Haga clic en el enlace enviado a su correo electrónico para restablecer su contraseña, el enlace es válido por solo 48 horas, por lo que necesita realizar la operación antes del final de su validez"
            };
        }

        public async Task<Response> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userRepository.SelectByEmail(model.Email);
            if (user is null)
            {
                throw new Exception("El usuario no existe");
            }

            string token = System.Web.HttpUtility.UrlDecode(model.Token);
            IdentityResult res = await _userRepository.ResetPassword(user, token, model.Password);
            if (!res.Succeeded)
            {
                throw new Exception("La contraseña no cambia, intente nuevamente más tarde");
            }

            return new Response
            {
                Status = "success",
                Message = "La contraseña ha sido cambiada con éxito"
            };
        }

        public async Task<Response> RegisterAdmin(RegisterModel model)
        {
            Exception e = new();
            var u = await _userRepository.SelectByEmail(model.Email);
            if (u != null)
            {
                e.Data.Add("email_error", "El correo electrónico ya existe");
                throw e;
            }

            u = await _userRepository.SelectBySocialNumber(model.SocialNumber);
            if (u != null)
            {
                e.Data.Add("socialNumber_error", "El número social ya existe");
                throw e;
            }

            User user = new()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = Regex.Replace(model.Name.ToLower(), @"\s+", ""),
                PhoneNumber = model.Phone,
                SocialNumber = model.SocialNumber,
                IsAdmin = true,
                DateCreated = DateTime.Now,
                Status = Status.Approuved,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userRepository.InsertAdmin(user, model.Password);
            if (result == null)
            {
                e.Data.Add("user", "Hay un problema con el registro del usuario, por favor verifica tus datos");
                throw e;
            }

            BankAccount account = new BankAccount()
            {
                AccountNumber = "Coop" + Guid.NewGuid().ToString("D"),
                Balance = 350.0,
                DateCreated = DateTime.Now,
                UserId = user.Id,
                Status = Status.Approuved
            };
            string bankAccount = await _bankRepository.InsertBankAccount(account);

            if (bankAccount == null)
            {
                e.Data.Add("user", "Hay un problema con la creación de BankAccount of User, intente nuevamente");
                throw e;
            }

            string token = await _userRepository.GenerateConfirmationToken(user);

            if (!await _mailService.SendConfirmMail(user, token))
            {
                e.Data.Add("user", "Hay un problema al enviar correo de confirmación, verifique su correo electrónico");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "Para completar su registro, se ha enviado un correo electrónico para confirmar su registro"
            };
        }
    }
}