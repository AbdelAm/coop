using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        async Task<Response> IUserService.register(RegisterModel model)
        {
            var u = await _userRepository.getUserByEmail(model.Email);
            if (u != null)
                throw new BadHttpRequestException("User already exists");

            User user = new()
            {
                UserName = model.Name,
                Email = model.Email,
                SocialNumber = model.SocialNumber,
                DateCreated = model.DateCreated,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            if (_userRepository.setUser(user, model.Password) == null)
                throw new Exception("There is probleme with registering user, please try again");

            return new Response { Status = "Success", Message = "User created successfully!, To complete your registration, An email has been sent to confirm your registration" };
        }
        async Task<TokenModel> IUserService.login(LoginModel model)
        {
            var user = await _userRepository.getUser(model);
            if (user != null && user.EmailConfirmed == true && user.Status == Status.Approuved)
            {

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
            throw new Exception("The user doesn't exists or it has not been approuved yet");
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var roles = await _userRepository.getUserRoles(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

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
