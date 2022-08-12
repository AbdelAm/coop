using coop2._0.Model;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IAuthentificationService
    {
        Task<Response> Register(RegisterModel model);
        Task<TokenModel> Login(LoginModel model);
        Task<Response> ConfirmUser(string param);
        Task<Response> ForgetPassword(ForgetPasswordModel model);
        Task<Response> ResetPassword(ResetPasswordModel model);
        Task<Response> RegisterAdmin(RegisterModel model);
    }
}