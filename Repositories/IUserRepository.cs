using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<string> SetUser(User user, string password);
        Task<User> GetUser(LoginModel model);
        Task<List<string>> GetUserRoles(User user);
        Task<ActionResult> RemoveUser(int id);
        Task<ActionResult<User>> RejectUser(int id);
        Task<ActionResult<User>> ValidateUser(int id);
        Task<ActionResult<User>> AddUser(UserModel model);
    }
}
