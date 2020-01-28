using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Niles.PrintWeb.Shared;
using Microsoft.Extensions.Logging;
using Niles.PrintWeb.Models.Settings;

namespace Niles.PrintWeb.Api.Services
{
    public class EmailService
    {
        private readonly EmailConnectionSettings _settings;
        private readonly ILogger _logger;

        public EmailService(ILogger logger, EmailConnectionSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                _logger.LogInformation("Try to create email message.");
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_settings.TeamName, _settings.Email));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
                _logger.LogInformation("Email message succesfully created");
                
                _logger.LogInformation("Try to send email message");
                await Task.Run(() =>
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect(_settings.SMTPHost, _settings.SSLPort, _settings.NeedSSL);
                        client.Authenticate(_settings.Email, _settings.Password);
                        client.Send(emailMessage);

                        client.Disconnect(true);
                    }
                });
                _logger.LogInformation("Email message succesfully sended.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public string CreateVerifyMailMessageContent(string subject, string message, string userLink)
        {
            string customizedMessage = $@"
                <div style='width: 100vh; height: 100vh; background-color: #f7f7f7;
                            margin: 0; font-size: 0.875rem; font-weight: 400;
                            line-height: 1.43; letter-spacing: 0.01071em;'
                >
                    <div style='margin:10% 20%;border-radius:5px;
                                flex-direction:column; align-items:center;
                                background-color:#ffffff; display:flex;'
                    >
                        <h1>{subject}</h1>
                        <span>{message}</span>
                        <a target='_blank' href='{userLink}' style='
                            background:#0068ff;border-radius:3px;
                            color:#ffffff;font-size:14px;font-weight:600;
                            padding:12px 20px 12px 20px;text-decoration:none'
                        >
                            Verify your email
                        </a>
                    </div>
                </div>
            ";

            return customizedMessage;
        }
    }
}