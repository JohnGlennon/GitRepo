using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BL.IdentityFramework
{
  public class EmailService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {

      var mailAdress = "IdentityMailServiceKdG@gmail.com";
      var sentFrom = "IdentityMailServiceKdG@gmail.com";
      var password = "IdentityMailServicePaswoord";

      SmtpClient client =
         new SmtpClient("smtp.gmail.com")
         {
           Port = 587,
           DeliveryMethod = SmtpDeliveryMethod.Network,
           UseDefaultCredentials = false
         };

      NetworkCredential credentials = new NetworkCredential(mailAdress, password);

      client.EnableSsl = true;
      client.Credentials = credentials;

      var mail = new MailMessage(sentFrom, message.Destination);

      mail.Subject = message.Subject;
      mail.Body = message.Body;

      return client.SendMailAsync(mail);
    }
  }
}
