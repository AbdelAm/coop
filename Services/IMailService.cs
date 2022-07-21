using coop2._0.Model;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IMailService
    {
        Task SendConfirmMail(MailModel mailer);
        Task SendForgetMail(MailModel mailer);
    }
}
