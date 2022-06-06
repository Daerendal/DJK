using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Text;

namespace Merkado.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public static string? currentUser { get; set; }

        public UserInfoController(MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [Authorize]
        public void addtoFavouritee(string UserId)
        {

            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;

            var ifObservedSeller = _db.FavouriteSeller.Any(O => O.SellerId == UserId && O.UserID == userId);
            if (ifObservedSeller)
            {
                removefavourite(UserId);
            }
            else
            {
                var ObservedSeller = new FavouriteSeller();
                ObservedSeller.SellerId = UserId;
                ObservedSeller.UserID = userId;
                _db.Add(ObservedSeller);
                _db.SaveChanges();
            }
        }
        public void removefavourite(string UserId)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
            var ifObservedSeller = _db.FavouriteSeller.Any(O => O.SellerId == UserId && O.UserID == userId);
            if (ifObservedSeller)
            {
                var ObservedSeller = _db.FavouriteSeller.Where(O => O.SellerId == UserId && O.UserID == userId).FirstOrDefault();
                _db.Remove(ObservedSeller);
                _db.SaveChanges();
            }
            else
            {
                addtoFavouritee(UserId);
            }
        }

        public IActionResult Index(string user)
        {
            ObservedSellerVM observedSellerVM = new ObservedSellerVM();
            if (user != null)
            {
                currentUser = user;
            }

            if (currentUser != null)
            {


                var userInfo = _db.Users
                              .Include(p => p.UserProducts.Where(s=>s.IsSold == false))
                              .Include(o => o.Opinions)
                              .Where(i => i.Id == currentUser)
                              .FirstOrDefault();

                var opinions = userInfo.Opinions;
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.Identity?.Name))
                {
                    var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
                    var ObservedSeller = _db.FavouriteSeller.Any(O => O.SellerId == user && O.UserID == userId);
                    var sameUser = userId == user;
                    ViewBag.IfalreadyObserved = ObservedSeller;
                    ViewBag.hideHeart = false;
                    if (sameUser)
                    {
                        ViewBag.hideHeart = true;
                    }
                }
                else
                {
                    ViewBag.IfalreadyObserved = false;
                    ViewBag.hideHeart = true;
                }
                if (opinions != null && opinions.Count > 0)
                {
                    foreach (Opinion opinion in opinions)
                    {
                        opinion.ReviewerName = _db.Users.Where(rev => rev.Id == opinion.ReviewerId).Select(n => n.FirstName).FirstOrDefault();

                    }

                    userInfo.Opinions = opinions;
                }
                observedSellerVM.Seller = userInfo;
                return View(observedSellerVM);
            }
            else
            {
                return View("ErrorPage");
            }
        }

        [Authorize]
        public async Task<IActionResult> AddOpinion(string comment, int rating)
        {
            //var lastOpinion = _db.Opinions.Include(x => x.OpinionId).OrderByDescending().Take(1);
            var logedUser = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            var opnionedUser = _userManager.FindByIdAsync(currentUser).Result;

            var opinion = new Opinion()
            {
                //OpinionId = 1,
                Comment = comment,
                Rate = rating,
                ReviewerId = logedUser.Id,
                ReviewerName = logedUser.UserName
            };

            PromoCodeGenerator();

            opnionedUser.Opinions.Add(opinion);

            _db.Opinions.Add(opinion);
            _db.Users.Update(opnionedUser);
            await _db.SaveChangesAsync();

            return Redirect("Index");
        }

        public void PromoCodeGenerator()
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            var promocodeCount = _db.Users.Where(id => id.Id == userId.Id).FirstOrDefault();
            if (promocodeCount.OpinionCounter == 3)
            {
                var n = 10; var k = 10;
                promocodeCount.OpinionCounter = 0;
                Random rand = new Random();
                // wypełnianie tablicy liczbami 1,2...n
                int[] numbers = new int[n];
                for (int i = 0; i < n; i++)
                    numbers[i] = i + 1;
                // losowanie k liczb

                for (int i = 0; i < k; i++)
                {
                    // tworzenie losowego indeksu pomiędzy 0 i n - 1
                    int r = rand.Next(n);

                    // wybieramy element z losowego miejsca
                    Console.WriteLine(numbers[r]);

                    // przeniesienia ostatniego elementu do miejsca z którego wzięliśmy
                    numbers[r] = numbers[n - 1];
                    n--;
                }
                var st = "";
                for(int i = 0; i < k; i++)
                {
                    st += numbers[i].ToString();
                }
                promocodeCount.PromoCode = st;

                    SendMail(st);
            }
            else
            {
                promocodeCount.OpinionCounter += 1;
            }
            _db.Users.Update(promocodeCount);
            _db.SaveChanges();
            
        }

        public void SendMail(string st)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            string to = userId.Email; //To address    
            string from = "MerkadoP4D2@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            var user = _db.Users
                    .Where(o => o.Id == userId.Id)
                    .FirstOrDefault();

            string mailbody = String.Format("Oto twój kod promocyjny {0}", st);
            message.Subject = String.Format("Promocyjny kod za 4 komentarz na dostawy na Merkado");
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
        }




    }
}
