using coop2._0.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(MailModel mailer)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("contact@coophalal.net"));
            mail.To.Add(MailboxAddress.Parse(mailer.Email));
            mail.Subject = mailer.Subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = mailer.Body };
            // create email message
            /*var mail = new MailMessage();
            mail.From = new MailAddress("register@coophalal.net");
            mail.Sender = new MailAddress(mailer.Email);
            mail.Subject = mailer.Subject;
            mail.IsBodyHtml = true;
            mail.Body = mailer.Body;*/

            // send email
            using var smtp = new SmtpClient();
                /*smtp.Host = _configuration["Mail:Host"];
                smtp.Port = Convert.ToInt32(_configuration["Mail:Port"]);
                smtp.Credentials = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
                smtp.Send(mail);*/
                smtp.Connect(_configuration["Mail:Host"], Convert.ToInt32(_configuration["Mail:Port"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["Mail:Email"], _configuration["Mail:Password"]);
                await smtp.SendAsync(mail);
                smtp.Disconnect(true);
            
        }
    }
}
