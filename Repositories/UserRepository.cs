using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> SelectByIdWithAccount(string id)
        {
            return await _userManager.Users.Where(u => u.Id == id).Include(u => u.BankAccounts).FirstOrDefaultAsync();
        }
        public async Task<User> SelectByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> SelectBySocialNumber(long socialNumber)
        {
            return await _userManager.Users.Where(u => u.SocialNumber == socialNumber).FirstOrDefaultAsync();
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

        public async Task<string> InsertAdmin(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
                await _userManager.AddToRoleAsync(user, "USER");
                return user.Id;
            }

            return null;
        }

        public async Task<User> SelectUser(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
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
            return await _userManager.Users.Where(u => !u.IsAdmin)
                .OrderByDescending(u => u.DateCreated)
                .Skip(page * PageSize)
                .Take(PageSize)
                .Select(u => new UserItemModel(u))
                .ToListAsync();
        }

        public async Task<int> SelectCount()
        {
            return await _userManager.Users.Where(u => !u.IsAdmin)
                .CountAsync();
        }

        public async Task<int> SelectProgressCount()
        {
            return await _userManager.Users.Where(u => !u.IsAdmin && u.Status == Status.Progress)
                .CountAsync();
        }

        public async Task<IEnumerable<UserItemModel>> SelectBy(string value)
        {
            return await _userManager.Users.Where(u =>
                    !u.IsAdmin && (u.Id.Contains(value) || u.Name.Contains(value) || u.Email.Contains(value)))
                .Select(u => new UserItemModel(u))
                .ToListAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            int count = await _userManager.Users.Where(u => u.Email == email)
                .CountAsync();
            return count > 0;
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUser(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
    }
}