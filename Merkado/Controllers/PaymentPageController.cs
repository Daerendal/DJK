using DJK.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DJK.ViewModels;
using Microsoft.AspNetCore.Identity;
using DJK.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DJK.Controllers
{


    public class PaymentPageController : Controller
    {
        private readonly ILogger<PaymentPageController> _logger;
        private readonly DJKDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PaymentPageController(ILogger<PaymentPageController> logger, DJKDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
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

        public int ValueBox(int checkboxId)
        {
            return (checkboxId);
        }
        public async Task<IActionResult> SendMails(string SellerId, int idprod, int deliveryValue)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            string to = userId.Email; //To address    
            string from = "DJKP4D2@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            var user = _db.Users
                    .Where(o => o.Id== SellerId)
                    .FirstOrDefault();
            var prod = _db.Products
                .Where(id => id.ProductId == idprod)
                .FirstOrDefault();
            var prov = _db.Providers
                .Where(v => v.ProviderId == deliveryValue)
                .FirstOrDefault();

            string mailbody = String.Format("Oto twoje potwierdzenie kupna przemiotu {0} od uzytkownika {1}, {2}. Metoda dostawy to: {3},Oceń sprzedawce aby otrzymać darmowe dostawy.", prod.Name, user.FirstName, user.Email, prov.Name);
            message.Subject = String.Format("Potwierdzenie Zakupu {0}, od sprzedawcy {1}", prod.Name, user.FirstName);
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("DJKP4D2", "ucxj srmh gxcy pmer");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                //client.Send(message);
                prod.IsSold = true;
                prod.BuyerId = userId.Id;
                _db.Products.Update(prod);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("/Home/Index");
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
    }

}
