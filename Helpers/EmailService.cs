namespace SiLoKonServer.Helpers
{
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Components.Forms;
    using MimeKit;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
                
        public async Task SendEmailAsync( string ime,string email, string body, string brojTelefona, IList<IBrowserFile> attachments)
        {
            string gPassword = _configuration["SmtpConfig:Password"];
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(ime, email));
            message.To.Add(new MailboxAddress("David", "david@silokon.com")); // Corrected line

            message.Subject = "Upit " + brojTelefona;
            

            var builder = new BodyBuilder();
            builder.TextBody = body +"\n" +brojTelefona;

            

            foreach (var attachment in attachments)
            {
                
                if (File.Exists(attachment.Name))
                {
                    builder.Attachments.Add(attachment.Name);
                }
            }
           
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false );
                await client.AuthenticateAsync("yubitubedeka@gmail.com", gPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
