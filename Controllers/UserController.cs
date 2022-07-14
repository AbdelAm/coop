using coop2._0.Entity;
using coop2._0.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Name);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new()
            {
                UserName = model.Name,
                Email = model.Email,
                SocialNumber = model.SocialNumber,
                CifNumber = model.CifNumber,
                DateCreated = model.DateCreated,
                IsAdmin = model.IsAdmin,
                EmailConfirmed = model.IsConfirmed,
                Status = model.Status,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            var role = _roleManager.FindByNameAsync("USER").Result;
            await _userManager.AddToRoleAsync(user, role.Name);
            return Ok(new Response { Status = "Success", Message = "User created successfully!, To complete your registration, An email has been sent to confirm your registration" });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Name);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var jwtSecurityToken = await CreateJwtToken(user);

                return Ok(new TokenModel { 
                    UserId = user.Id,
                    UserName = user.Name,
                    IsAdmin = user.IsAdmin,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ValidTo = jwtSecurityToken.ValidTo
                });
            }
            return Unauthorized();
        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id)
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
