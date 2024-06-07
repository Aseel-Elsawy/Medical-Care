using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserManagmentService.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;




namespace UserManagmentService.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfigration _emailconfig;
        public EmailService(EmailConfigration emailConfig)
        {
            _emailconfig = emailConfig;
        }
        public void SendEmail(Message message)
        {
          var emailMessage=CreateEmailMessage(message);
            Send(emailMessage);
        }
       
        public void Send(MimeMessage mailmessage)
        {
            using var client = new SmtpClient();
            try
            {
              client.Connect(_emailconfig.Smtpserver,_emailconfig.port,true);
                client.AuthenticationMechanisms.Remove("XQAUTH2");
                client.Authenticate(_emailconfig.UserName,_emailconfig.Password);
                client.Send(mailmessage);
            }
            catch { throw; }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email",_emailconfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) {Text=message.Content };
            return emailMessage;
        }
    }
}
