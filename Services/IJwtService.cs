using coop2._0.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IJwtService
    {
        Task<JwtSecurityToken> GenerateJwtToken(User user);
    }
}
