using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PasswordPortalApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmail(string userEmail, string token)
        {
            try
            {
                var emailSettings = _config.GetSection("Email");

                string smtpServer = emailSettings["SmtpServer"];
                int smtpPort = int.Parse(emailSettings["SmtpPort"]);
                string fromAddress = emailSettings["FromAddress"];
                string username = emailSettings["Username"];
                string password = emailSettings["Password"];
                string auditBcc = emailSettings["AuditBcc"];

                // Build password reset link (adjust host/port for production)
                string resetLink = $"https://localhost:5096/ResetPassword?token={token}";

                var mail = new MailMessage();
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(userEmail);

                if (!string.IsNullOrEmpty(auditBcc))
                {
                    mail.Bcc.Add(auditBcc);  // Audit BCC to track all sent emails
                }

                mail.Subject = "Password Reset Request";
                mail.Body = $@"Hello,

A password reset was requested for your account.

Click the link below to reset your password:
{resetLink}

If you did not request this, please ignore this email.";

                using var smtp = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(username, password)
                };

                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send password reset email: {ex.Message}", ex);
            }
        }
    }
}
