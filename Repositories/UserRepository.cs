using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<User> getUserByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<string> setUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");
                return user.Id;
            }
            return null;
        }
        public async Task<User> getUser(LoginModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Cif);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }
            return null;
        }
        public async Task<List<string>> getUserRoles(User user)
        {
            return (List<string>) await _userManager.GetRolesAsync(user);
        }
    }
}
