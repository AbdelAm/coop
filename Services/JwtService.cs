using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class JwtService : IJwtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public JwtService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<TokenModel> GenerateJwtToken(User user)
        {
            var roles = await _userRepository.SelectUserRoles(user);
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
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:DurationInDays"])),
                signingCredentials: signingCredentials);

            return new TokenModel
            {
                Cif = user.Id,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
<<<<<<< HEAD
=======
                BankAccount = user.BankAccounts.First().Id,
>>>>>>> e873bdad2866617f81a51c169c19d06a11d9f5e2
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ValidTo = jwtSecurityToken.ValidTo
            };
        }

        public string DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.First(claim => claim.Type == "uid").Value;
        }
    }
}