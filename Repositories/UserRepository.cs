using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const int PageSize = 5;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<User> SelectById(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<User> SelectByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<User> SelectBySocialNumber(string socialNumber)
        {
            User user = await _userManager.Users.Where(u => u.SocialNumber == socialNumber)
                                                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<string> InsertUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "USER");
                return user.Id;
            }

            return null;
        }

        public async Task<User> SelectUser(LoginModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Cif);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<List<string>> SelectUserRoles(User user)
        {
            return (List<string>)await _userManager.GetRolesAsync(user);
        }

        public async Task<string> GenerateConfirmationToken(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GenerateResetToken(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmail(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> ResetPassword(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IEnumerable<UserItemModel>> SelectAll(int page)
        {
            return await _userManager.Users.Where(u => u.EmailConfirmed && u.Status != Status.Approuved)
                                           .Skip(page*PageSize)
                                           .Take(PageSize)
                                           .Select(u => new UserItemModel(u))
                                           .ToListAsync();
        }

        public async Task<int> SelectCount()
        {
            return await _userManager.Users.Where(u => u.EmailConfirmed && u.Status != Status.Approuved)
                                           .CountAsync();
        }

        public async Task<IEnumerable<UserItemModel>> SelectBy(string value)
        {
            return await _userManager.Users.Where(u => !u.IsAdmin && (u.Id.Contains(value) || u.Name.Contains(value) || u.Email.Contains(value)))
                                           .Select(u => new UserItemModel(u))
                                           .ToListAsync();
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUser(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<ActionResult> RemoveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.Status == Status.Approuved) return null;

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

        public async Task<ActionResult<User>> ValidateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is not { Status: Status.Progress }) return null;
            user.Status = Status.Approuved;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}