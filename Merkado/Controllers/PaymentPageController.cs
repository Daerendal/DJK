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
            string to = "aleksanderjokiel@gmail.com"; //To address    
            string from = "MerkadoP4D2@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "OLEK TO DZIAŁA";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("MerkadoP4D2", "ucxj srmh gxcy pmer");
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

            return Redirect("Index");
        }
        public string promocode(string promocode)
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
            var code = user.PromoCode;
            if(promocode == code)
            {
                return "activecode";
            }
            else
            {
                return "inactivecode";
            }
       
        }
        protected string wartosc(string code)
        {

            {
                return code;
            }
        }
        //public string promocode(string promocode)
        //{
        //    var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        //    string userId = string.Empty;

        //    if (userName != null)
        //    {
        //        userId = _userManager.FindByNameAsync(userName).Result.Id;

        //    }
        //    var user = _db.Users
        //           .Where(id => id.Id == userId)
        //           .FirstOrDefault();
        //    var code = user.PromoCode;
        //    if(promocode == code)
        //    {
        //        return "activecode";
        //    }
        //    else
        //    {
        //        return "inactivecode";
        //    }
       
        //}
        //protected string wartosc(string code)
        //{

        //    {
        //        return code;
        //    }
        //}
    }

}
