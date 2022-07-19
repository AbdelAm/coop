using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<string> SetUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");
                return user.Id;
            }

            return null;
        }

        public async Task<User> GetUser(LoginModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Cif);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            return (List<string>)await _userManager.GetRolesAsync(user);
        }
        public async Task<ActionResult> RemoveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.Status == Status.Approved) return null;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<ActionResult<User>> RejectUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is not { Status: Status.Progress }) return null;
            user.Status = Status.Rejected;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<ActionResult<User>> AddUser(UserModel model)
        {

            var user = new User()
            {
                SocialNumber = model.SocialNumber,
                IsAdmin=model.IsAdmin,
                DateCreated = DateTime.Now,
                Status = model.Status
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<ActionResult<User>> ValidateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is not { Status: Status.Progress }) return null;
            user.Status = Status.Approved;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}