using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

            if(currentUser != null)
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
                    if(sameUser)
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

            opnionedUser.Opinions.Add(opinion);

            _db.Opinions.Add(opinion);
            _db.Users.Update(opnionedUser);
            await _db.SaveChangesAsync();

            return Redirect("Index");
        }



    }
}
