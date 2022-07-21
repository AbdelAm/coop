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
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string id);
        Task<string> SetUser(User user, string password);
        Task<User> GetUser(LoginModel model);
        Task<List<string>> GetUserRoles(User user);
        Task<string> GenerateConfirmationToken(User user);
        Task<string> GenerateResetToken(User user);
        Task<IdentityResult> ConfirmEmail(User user, string token);
        Task<ActionResult> RemoveUser(int id);
        Task<ActionResult<User>> RejectUser(int id);
        Task<ActionResult<User>> ValidateUser(int id);
        Task<ActionResult<User>> AddUser(UserModel model);
    }
}
