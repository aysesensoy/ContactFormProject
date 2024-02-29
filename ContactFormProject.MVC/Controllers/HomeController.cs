using ContactFormProject.Data;
using ContactFormProject.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactFormProject.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private MailRepository _mailRepository;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetSection("ConnectionStrings").Value;
            _mailRepository = new MailRepository(connectionString);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MailModel mailModel)
        {
            #region Admine bilgilendirme maili gönderimi
            string adminSenderMailAddress = _configuration.GetSection("EmailParameters").GetSection("AdminSenderMailAddress").Value;
            string senderMailAddress = _configuration.GetSection("EmailParameters").GetSection("SenderMailAddress").Value;
            string senderMailPassword = _configuration.GetSection("EmailParameters").GetSection("SenderMailPassword").Value;
            string mailHost = _configuration.GetSection("EmailParameters").GetSection("MailHost").Value;
            int mailPort = Convert.ToInt32(_configuration.GetSection("EmailParameters").GetSection("MailPort").Value);
            mailModel.FromAddress = senderMailAddress;
            mailModel.ToAddress = adminSenderMailAddress;
            MailHelper mailHelper = new MailHelper(mailHost, mailPort, senderMailAddress, senderMailPassword);
            bool resultSendMail = mailHelper.SendEmail(mailModel, true);
            #endregion

            #region Veritabanına kayıt atılması
            int resultInsertData = 0;
            if (resultSendMail)
            {
 
                Mail mail = new Mail
                {
                    Content = mailModel.Content,
                    Subject = mailModel.Subject,
                    GuestFirstName = mailModel.GuestFirstName,
                    GuestLastName = mailModel.GuestLastName,
                    GuestMailAddress = mailModel.GuestMailAddress,
                    SendDate = DateTime.Now,
                    IsIncomingMail = true
                };

                resultInsertData = _mailRepository.Create(mail);
            }
            #endregion

            if (resultInsertData > 0)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}