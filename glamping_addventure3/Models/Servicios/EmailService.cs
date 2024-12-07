using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Glamping_Addventure3.Services;

namespace Glamping_Addventure3.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoRecuperacion(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Isabela", _configuration["Email:SmtpUsername"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    var smtpServer = _configuration["Email:SmtpServer"];
                    var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
                    var smtpUser = _configuration["Email:SmtpUsername"];
                    var smtpPass = _configuration["Email:SmtpPassword"];

                    // Conexión al servidor SMTP
                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                    // Autenticación
                    await client.AuthenticateAsync(smtpUser, smtpPass);

                    // Enviar el mensaje
                    await client.SendAsync(emailMessage);
                    Console.WriteLine("Correo enviado exitosamente.");

                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                }
            }
        }
    }
}
