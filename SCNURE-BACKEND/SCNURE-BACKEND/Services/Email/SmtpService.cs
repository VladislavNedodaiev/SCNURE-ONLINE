using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Services.Email
{
	public class SmtpService : IEmailService
	{
		private readonly IConfiguration _configuration;

		public SmtpService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var emailMessage = new MimeMessage();
			IConfigurationSection smtpMailSettings = _configuration.GetSection("SmtpMailSettings");

			emailMessage.From.Add(new MailboxAddress(smtpMailSettings.GetValue<string>("SenderName"), smtpMailSettings.GetValue<string>("SenderAddress")));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message
			};

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync(smtpMailSettings.GetValue<string>("SmtpServer"), smtpMailSettings.GetValue<int>("SmtpPort"), smtpMailSettings.GetValue<bool>("UseSsl"));
				await client.AuthenticateAsync(smtpMailSettings.GetValue<string>("SmtpUsername"), smtpMailSettings.GetValue<string>("SmtpPassword"));
				await client.SendAsync(emailMessage);

				await client.DisconnectAsync(true);
			}
		}
	}
}
