using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Tls;
using System.Net.Mail;
using System.Net;
using Repository.Settings;
using Entity;

namespace EPAYGOLF.Models
{
	public  class EmailService
	{

		public static string SenderEmail = "";
		public static string Password = "";
		public static string ServerName = "";
		public static int Port =0;
		private ISettingsRepository _settingsRepository = new SettingsRepository();

		public  void SendSMTPEmail(string emailto, string subject, string body,string attachmentpath="")
		{

			var model = _settingsRepository.GetSettingsList().FirstOrDefault();
			if (model == null)
			{
				Helpers.ApplicationExceptions.SaveActivityLog("SMTP Config is missing");
				return;
			}
			else
			{
				SenderEmail = model.SMTP_SenderEmail;
				Password = model.SMTP_Password;
				ServerName = model.SMTP_ServerName;
				Port = Convert.ToInt32(model.SMTP_Port);
			}
			// Create a MailMessage object


			MailMessage mail = new MailMessage(SenderEmail, emailto);
			mail.Subject = subject;
			mail.Body = body;

			// Create the attachment and add it to the email
			if (!string.IsNullOrEmpty(attachmentpath))
			{
				var attachment = new Attachment(attachmentpath);
				mail.Attachments.Add(attachment);
			}

			// Configure the SMTP client
			SmtpClient smtpClient = new SmtpClient(ServerName); // Replace with your SMTP server
			smtpClient.Port = Port; // Port number for the SMTP server
			smtpClient.EnableSsl = true; // Enable SSL/TLS
			smtpClient.Credentials = new NetworkCredential(SenderEmail, Password);
			//smtpClient.UseDefaultCredentials = false;


			try
			{
				// Send the email
				smtpClient.Send(mail);
				Helpers.ApplicationExceptions.SaveActivityLog($"Email sent successfully to  email {emailto} at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
				Console.WriteLine("Email sent successfully!");
			}
			catch (Exception ex)
			{
				Helpers.ApplicationExceptions.SaveAppError(ex);
				Helpers.ApplicationExceptions.SaveActivityLog($"Failed to send email  email {emailto} at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
			}

		}
	}
}
