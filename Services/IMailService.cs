using coop2._0.Entities;
using coop2._0.Model;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IMailService
    {
        Task<bool> SendConfirmMail(User user, string token);
        Task<bool> SendForgetMail(User user, string token);
        Task<bool> SendValidationMail(MailModel mailer);
    }
}
