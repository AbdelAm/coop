using coop2._0.Entities;
using coop2._0.Model;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IJwtService
    {
        Task<TokenModel> GenerateJwtToken(User user);
        string DecodeJwtToken(string jwt);
    }
}
