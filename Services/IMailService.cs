using coop2._0.Model;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IMailService
    {
        Task SendEmail(MailModel mailer);
    }
}
