using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "teste",
        string fromEmail = "emailteste@gmail.com")
    {
        //initialize smtpClient
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

        //use password through Credentials
        smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //use safe port
        smtpClient.EnableSsl = true;

        //create mail message
        var mail = new MailMessage();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        //send mail
        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
