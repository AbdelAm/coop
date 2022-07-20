﻿using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IMailService mailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task<Response> Register(RegisterModel model)
        {
            var u = await _userRepository.GetUserByEmail(model.Email);
            Exception e = new();
            if (u != null)
            {
                e.Data.Add("email_error", "email is already existe");
                throw e;
            }
            

            User user = new()
            {
                Name = model.Name,
                Email = model.Email,
                UserName =  Regex.Replace(model.Name.ToLower(), @"\s+", ""),
                PhoneNumber = model.Phone,
                SocialNumber = model.SocialNumber,
                DateCreated = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userRepository.SetUser(user, model.Password);
            if (result == null)
            {
                e.Data.Add("user", "There is problem with registering user, please try again");
                throw e;
            }

            string token = await _userRepository.GenerateConfirmationToken(user);
            MailModel mailModel = new MailModel()
            {
                Email = model.Email,
                Subject = "Email Confirmation",
                Body = user.Name + '-' + token + '-' + user.Email,
            };
            await _mailService.SendEmail(mailModel);
            
            return new Response
            {
                Status = "success",
                Message = "To complete your registration, An email has been sent to confirm your registration"
            };
        }

        public async Task<TokenModel> Login(LoginModel model)
        {
            var user = await _userRepository.GetUser(model);
            if (user is null)
                throw new Exception("The user doesn't exist");
            if (user is not { EmailConfirmed: true })
                throw new Exception("The user has not been confirmed yet");
            if (user is not { Status: Status.Approuved })
                throw new Exception("The user has not been approved yet");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new TokenModel
            {
                Cif = user.Id,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ValidTo = jwtSecurityToken.ValidTo
            };
        }

        public async Task<Response> ConfirmUser(string param)
        {
            var values = param.Split('-');
            var user = await _userRepository.GetUserByEmail(values[1]);
            if(user is not { EmailConfirmed: false, Status: Status.Progress })
            {
                throw new Exception("The user is already confirmed");
            }
            var token = values[0].Replace(' ', '+');
            IdentityResult res = await _userRepository.ConfirmEmail(user, token);
            if(!res.Succeeded)
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

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var roles = await _userRepository.GetUserRoles(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Cif", user.Id)
                }
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:DurationInMinutes"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}