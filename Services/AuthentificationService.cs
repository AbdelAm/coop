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

        public AuthentificationService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration, IMailService mailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _mailService = mailService;
        }
        public async Task<Response> Register(RegisterModel model)
        {
            Exception e = new();
            var u = await _userRepository.SelectByEmail(model.Email);
            if (u != null)
            {
                e.Data.Add("email_error", "email is already existe");
                throw e;
            }
            u = await _userRepository.SelectBySocialNumber(model.SocialNumber);
            if (u != null)
            {
                e.Data.Add("socialNumber_error", "Social Number is already existe");
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
                e.Data.Add("user", "There is problem with registering user, please try again");
                throw e;
            }

            string token = await _userRepository.GenerateConfirmationToken(user);
            
            if(!await _mailService.SendConfirmMail(user, token))
            {
                e.Data.Add("user", "There is problem with sending confirmation Mail, please verify your email");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "To complete your registration, An email has been sent to confirm your registration"
            };
        }

        public async Task<TokenModel> Login(LoginModel model)
        {
            var user = await _userRepository.SelectUser(model);

            if (user is null)
                throw new Exception("The user doesn't exist");
            if (user is not { EmailConfirmed: true })
                throw new Exception("The user has not been confirmed yet");
            if (user is not { Status: Status.Approuved })
                throw new Exception("The user has not been approved yet");

            return await _jwtService.GenerateJwtToken(user);
        }

        public async Task<Response> ConfirmUser(string param)
        {
            var values = param.Split('-');
            var user = await _userRepository.SelectByEmail(values[1]);
            if (user is not { EmailConfirmed: false })
            {
                throw new Exception("The user is already confirmed");
            }
            string token = values[0].Replace(' ', '+');
            IdentityResult res = await _userRepository.ConfirmEmail(user, token);
            if (!res.Succeeded)
            {
                throw new Exception("the email hasn't been confirmed, please try again");
            }

            return new Response
            {
                Status = "Success",
                Message =
                    "User is confirmed successfully"
            };
        }
        public async Task<Response> ForgetPassword(ForgetPasswordModel model)
        {
            var user = await _userRepository.SelectByEmail(model.Email);
            Exception e = new();
            if (user == null)
            {
                e.Data.Add("email_error", "email does not existe, please verify your information");
                throw e;
            }

            string token = await _userRepository.GenerateResetToken(user);
            token = System.Web.HttpUtility.UrlEncode(token);

            if(!await _mailService.SendForgetMail(user, token))
            {
                e.Data.Add("email_error", "There is problem with sending Reset Password Mail, please verify your email");
                throw e;
            }

            return new Response
            {
                Status = "success",
                Message = "Click on link sended to your email to reset your password, the link is valid for only 48 hours so you need make the operation before the end of its validity"
            };
        }

        public async Task<Response> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userRepository.SelectByEmail(model.Email);
            if (user is null)
            {
                throw new Exception("The user does not existe");
            }
            string token = System.Web.HttpUtility.UrlDecode(model.Token);
            IdentityResult res = await _userRepository.ResetPassword(user, token, model.Password);
            if (!res.Succeeded)
            {
                throw new Exception("the password doesn't change, please try again later");
            }

            return new Response
            {
                Status = "Success",
                Message = "The password has been changed successfully"
            };
        }
    }
}
