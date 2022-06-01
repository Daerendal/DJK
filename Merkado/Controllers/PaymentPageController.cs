using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Merkado.Controllers
{
    public class PaymentPageController : Controller
    {
        private readonly ILogger<PaymentPageController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PaymentPageController(ILogger<PaymentPageController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
            

        public IActionResult Index(int item)
        {
            var paymentPageVM = new PaymentPageVM();



            if (item > 0)
            {

                var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                string userId = string.Empty;

                if (userName != null)
                {
                    userId = _userManager.FindByNameAsync(userName).Result.Id;

                }

                var user = _db.Users
                    .Where(id => id.Id == userId)
                    .FirstOrDefault();

                paymentPageVM.Buyer = user;

                var product = _db.Products
                                 .Include(i => i.Images)
                                 .Include(p => p.Providers)
                                 .Include(c => c.Category)
                                 .Include(i => i.Images)                                
                                 .Where(id => id.ProductId == item)
                                 .FirstOrDefault();
                ViewBag.Andrzej = Math.Round((float)product.Price + 11.99, 2);

                paymentPageVM.CurrentProduct = product;

                if (product != null)
                {
                    paymentPageVM.Seller = _db.Users.Where(id => id.UserProducts.Contains(product)).FirstOrDefault();




                    return View(paymentPageVM);
                }

                else
                {
                    return NotFound();
                }



            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> SendMail(PaymentPageVM model)
        {
            //PIERWSZY SPOSÓB - TIME OUT
            string to = "merkadop4d2@gmail.com"; //To address    
            string from = "merkadop4d2@wp.pl"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "In this article you will learn how to send a email using Asp.Net & C#";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.wp.pl", 465); //Yahoo smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("merkadop4d2@wp.pl", "P4D2Projekt123!");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //DRUGI SPOSÓB - NIE DZIAŁA SMTP
            MailMessage mail = new MailMessage();
            mail.To.Add("merkadop4d2@gmail.com");
            mail.To.Add("aleksanderjokiel@gmail.com");
            mail.From = new MailAddress("merkadop4d2@wp.pl");
            mail.Subject = "Testowy mail";

            string Body = "Tresc smiesznego" +
                          "maila testowego";
            mail.Body = Body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.wp.pl"; //Or Your SMTP Server Address
            smtp.Credentials = new System.Net.NetworkCredential
                 ("merkadop4d2@wp.pl", "P4D2Projekt123!");
            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            smtp.Send(mail);

            //KONIEC 

            return Redirect("Index");
        }
    }
}
