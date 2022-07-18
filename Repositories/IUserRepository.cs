using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IUserRepository
    {
        Task<User> getUserByEmail(string email);
        Task<string> setUser(User user, string password);
        Task<User> getUser(LoginModel model);
        Task<List<string>> getUserRoles(User user);
    }
}
