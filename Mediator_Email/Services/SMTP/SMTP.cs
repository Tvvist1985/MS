using MimeKit;
using MailKit.Net.Smtp;

namespace Mediator_Email.Services.SMTP
{
    public class SMTP :ISMTP
    {
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            using var client = new SmtpClient();
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Administration of CUBY", "fl-13@mail.ru"));  //Who is send mess
                emailMessage.To.Add(new MailboxAddress("", email));  //Where to send mess
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)  //Message
                {
                    Text = message
                };

                //Гугловский  порт 465 or 587
                await client.ConnectAsync("smtp.mail.ru", 465, true);  //connect to server 
                await client.AuthenticateAsync("fl-13@mail.ru", "nciazdjjDA5FBM8vACAt");  //Athenticate to service  (need register to service)
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public string SendPinCodeToEmail(in string email)
        {
            string randNumber = new Random().Next(1000, 9999).ToString();           
            SendEmailAsync(email, "Subject message.", $"Registration number:" + randNumber);

            return randNumber;
        }

        public void RecoveryPassword(in string email, in string message)
        {
            SendEmailAsync(email, "Subject message.", $"Registration number:" + message);
        }
    }
}
