using System;
using System.Net;
using System.Net.Mail;
using JustAnEmailClient.Models;

namespace JustAnEmailClient.Services;

public class MailSender
{
    public MailSender() {}
    // Create a struct or similar to handle all the needed data from the form
    public void SendEmail(UserInfo userInfo, string sentTo = "", string subject = "", string msgBody = "")
    {
        // handle no subject/no destination -> or handle in the UI directly

        SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com");
        smtpClient.Port = 587;
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.UseDefaultCredentials = false;
        NetworkCredential credentials = new NetworkCredential(userInfo.email, userInfo.password);

        smtpClient.EnableSsl = true;
        smtpClient.Credentials = credentials;

        MailMessage mailMessage = new MailMessage(userInfo.email, sentTo);
        mailMessage.Subject = subject;
        mailMessage.Body = msgBody;

        smtpClient.Send(mailMessage);
    }
}
