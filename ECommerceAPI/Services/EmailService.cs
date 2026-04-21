using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using ECommerceAPI.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog.Core;
using MailKit.Net.Smtp;

namespace ECommerceAPI.Services
{
    public class EmailService: IEmailService
    {
        private EmailSettings emailSettings;
        private ILogger<EmailService> _logger;
        private AppDbContext db;
        private ICacheService redis;
        public EmailService(AppDbContext db, IOptions<EmailSettings> emailsettings ,ILogger<EmailService> _logger, ICacheService redis)
        {
            this.emailSettings = emailsettings.Value;
            this._logger = _logger;
            this.db = db;
            this.redis = redis;
        }

        async public Task SendOtp(string email, int otp)
        {
            MimeMessage mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.SenderEmail));
            mail.To.Add(new MailboxAddress(null,email));
            mail.Subject = "Confirm Account Creation";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <html>
                <body>
                    <h1>Verify OTP in the App</h1>
                    <h2>Your OTP Is</h2>
                    <h3>{otp}</h3>
                </body>
                </html>";
            mail.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtp = new SmtpClient();
            await smtp.ConnectAsync(
                emailSettings.Host,
                emailSettings.Port,
                SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                emailSettings.SenderEmail,
                emailSettings.AppPassword);

            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Email}",email);
        }
        async public Task ResendOTP(string email)
        {
            string? otp = redis.Get($"otp:{email}");
            if(otp != null)
            {
                await SendOtp(email, int.Parse(otp));
                _logger.LogInformation("Email resent successfully to {Email}", email);
            }
            _logger.LogInformation("Error resending email: {Email}", email);
        }
    }
}
