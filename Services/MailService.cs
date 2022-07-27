using coop2._0.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
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

        private async Task SetUpSmtp(MimeMessage mail)
        {
            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["Mail:Host"], Convert.ToInt32(_configuration["Mail:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            await smtp.SendAsync(mail);
            smtp.Disconnect(true);
        }
        public async Task SendConfirmMail(MailModel mailer)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("contact@coophalal.net"));
            mail.To.Add(MailboxAddress.Parse(mailer.Email));
            mail.Subject = mailer.Subject;
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("./Mails/ConfirmMail.cshtml"))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }
            var values = mailer.Body.Split('-', 2);

            string messageBody = string.Format(builder.HtmlBody,
                        values[0],
                        values[1]
                        );
            mail.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            await SetUpSmtp(mail);
        }

        public async Task SendForgetMail(MailModel mailer)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("contact@coophalal.net"));
            mail.To.Add(MailboxAddress.Parse(mailer.Email));
            mail.Subject = mailer.Subject;
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("./Mails/ForgetMail.cshtml"))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }
            var values = mailer.Body.Split('-', 2);

            string messageBody = string.Format(builder.HtmlBody,
                        values[0],
                        values[1]
                        );
            mail.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            await SetUpSmtp(mail);
        }
        public async Task SendValidationMail(MailModel mailer)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("contact@coophalal.net"));
            mail.To.Add(MailboxAddress.Parse(mailer.Email));
            mail.Subject = mailer.Subject;
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("./Mails/ValidationMail.cshtml"))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }
            var values = mailer.Body.Split('-', 2);

            string messageBody = string.Format(builder.HtmlBody,
                        values[0],
                        values[1]
                        );
            mail.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            await SetUpSmtp(mail);
        }
    }
}
