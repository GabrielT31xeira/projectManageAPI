using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace API.projectManager.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;

        public EmailService(string smtpHost, int smtpPort, string username, string password)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _username = username;
            _password = password;
        }

        public void SendEmail(string toAddress, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", _username));
            message.To.Add(MailboxAddress.Parse(toAddress));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(_username, _password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
