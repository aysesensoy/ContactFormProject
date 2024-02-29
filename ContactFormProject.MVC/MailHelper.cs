using System.Net.Mail;
using System.Net;
using ContactFormProject.MVC.Models;

namespace ContactFormProject.MVC
{
    public class MailHelper
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;

        public MailHelper(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }

        public bool SendEmail(MailModel model, bool isAdminNotificationMail)
        {
            MailMessage mailMessage = new()
            {
                Subject = model.Subject
            };

            mailMessage.To.Add(new MailAddress(model.ToAddress));
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;
            mailMessage.From = new MailAddress(model.FromAddress, _username);

            if (isAdminNotificationMail)
            {
                mailMessage.Body = $"{model.GuestFirstName} {model.GuestLastName} kişisinden {model.Subject} konusu ile aşağıdaki ileti gönderilmiştir: <br/> {model.Content}  <br/> Gönderen kişinin eposta adresi : {model.GuestMailAddress}";
            }
            else
            {
                mailMessage.Body = model.Content;
            }

            SmtpClient smtpClient = new SmtpClient(_host, _port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            NetworkCredential AccountInfo = new NetworkCredential(model.FromAddress, _password);
            smtpClient.Credentials = AccountInfo;

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch(Exception exception) 
            {
                return false;
            }
        }
    }
}
