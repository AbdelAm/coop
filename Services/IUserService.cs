using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IUserService
    {
        Task<Response> register(RegisterModel model);
        Task<TokenModel> login(LoginModel model);
    }
}
