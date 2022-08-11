using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IUserRepository
    {
        Task<User> SelectById(string id);
        Task<User> SelectByEmail(string email);
        Task<User> SelectBySocialNumber(string socialNumber);
        Task<string> InsertUser(User user, string password);
        Task<string> InsertAdmin(User user, string password);
        Task<User> SelectUser(LoginModel model);
        Task<List<string>> SelectUserRoles(User user);
        Task<string> GenerateConfirmationToken(User user);
        Task<string> GenerateResetToken(User user);
        Task<IdentityResult> ConfirmEmail(User user, string token);
        Task<IdentityResult> ResetPassword(User user, string token, string password);
        Task<IEnumerable<UserItemModel>> SelectAll(int page);
        Task<int> SelectCount();
        Task<IEnumerable<UserItemModel>> SelectBy(string value);
        Task<bool> EmailExists(string email);
        Task<bool> CheckPassword(User user, string password);
        Task<IdentityResult> UpdateUser(User user);
        Task<IdentityResult> DeleteUser(User user);
    }
}
