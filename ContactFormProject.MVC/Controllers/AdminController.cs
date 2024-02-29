using ContactFormProject.Data;
using ContactFormProject.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace ContactFormProject.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private MailRepository _mailRepository;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetSection("ConnectionStrings").Value;
            _mailRepository = new MailRepository(connectionString);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            string adminUsername = _configuration.GetSection("LoginParameters").GetSection("AdminUsername").Value;
            string adminPassword = _configuration.GetSection("LoginParameters").GetSection("AdminPassword").Value;

            if (loginModel.Username != adminUsername || loginModel.Password != adminPassword)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı. Lütfen giriş bilgilerinizi kontrol ediniz.");
                return View();
            }
            else
            {
                HttpContext.Session.SetString("Username", "Admin");
                return RedirectToAction("IncomingMails", "Admin");
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult IncomingMails()
        {
            if (CheckAdminAccess())
            {
                return RedirectToAction("Login", "Admin");
            }

            List<Mail> incomingMails = _mailRepository.GetIncomingMails();
            List<MailModel> model = new List<MailModel>();

            foreach (var mail in incomingMails)
            {
                MailModel mailModel = EntityToModel(mail);
                model.Add(mailModel);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult OutgoingMails()
        {
            if (CheckAdminAccess())
            {
                return RedirectToAction("Login", "Admin");
            }

            List<Mail> outgoingMails = _mailRepository.GetOutgoingMails();
            List<MailModel> model = new List<MailModel>();

            foreach (var mail in outgoingMails)
            {
                MailModel mailModel = EntityToModel(mail);
                model.Add(mailModel);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ReadMail(int id)
        {
            if (CheckAdminAccess())
            {
                return RedirectToAction("Login", "Admin");
            }

            Mail mail = _mailRepository.GetMail(id);
            MailModel model = EntityToModel(mail);
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteMail(int id)
        {
            if (CheckAdminAccess())
            {
                return RedirectToAction("Login", "Admin");
            }

            int result = _mailRepository.Delete(id);
            if (result > 0)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }


        [HttpPost]
        public IActionResult Reply(MailModel mailModel)
        {
            if (CheckAdminAccess())
            {
                return RedirectToAction("Login", "Admin");
            }

            string adminSenderMailAddress = _configuration.GetSection("EmailParameters").GetSection("AdminSenderMailAddress").Value;
            string adminSenderMailPassword = _configuration.GetSection("EmailParameters").GetSection("AdminSenderMailPassword").Value;
            string mailHost = _configuration.GetSection("EmailParameters").GetSection("MailHost").Value;
            int mailPort = Convert.ToInt32(_configuration.GetSection("EmailParameters").GetSection("MailPort").Value);
            mailModel.ToAddress = mailModel.GuestMailAddress;
            mailModel.FromAddress = adminSenderMailAddress;
            MailHelper mailHelper = new MailHelper(mailHost, mailPort, adminSenderMailAddress, adminSenderMailPassword);
            bool resultSendMail = mailHelper.SendEmail(mailModel, false);
            if (resultSendMail)
            {
                Mail mail = new Mail
                {
                    Content = mailModel.Content,
                    Subject = $"Yanıt - {mailModel.Subject}",
                    GuestFirstName = mailModel.GuestFirstName,
                    GuestLastName = mailModel.GuestLastName,
                    GuestMailAddress = mailModel.GuestMailAddress,
                    SendDate = DateTime.Now,
                    IsIncomingMail = false
                };

                _mailRepository.Create(mail);
            }
            
            return RedirectToAction("OutgoingMails", "Admin");
        }

        private static MailModel EntityToModel(Mail mail)
        {
            return new MailModel
            {
                Id = mail.Id,
                GuestFirstName = mail.GuestFirstName,
                GuestLastName = mail.GuestLastName,
                GuestMailAddress = mail.GuestMailAddress,
                Subject = mail.Subject,
                Content = mail.Content,
                IsRead = mail.IsRead,
                IsIncomingMail = mail.IsIncomingMail,
                SendDate = mail.SendDate
            };
        }

        private bool CheckAdminAccess()
        {
            return HttpContext.Session.GetString("Username") != "Admin";
        }
    }
}
