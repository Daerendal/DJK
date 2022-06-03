using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;

namespace Merkado.Controllers
{
    public class ObservedSellerController : Controller
    {
        private readonly ILogger<ObservedSellerController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ObservedSellerController(ILogger<ObservedSellerController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult StopObserved(string Id)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
            var ifObservedUser = _db.FavouriteSeller.Any(O => O.SellerId == Id && O.UserID == userId);
            if (ifObservedUser)
            {
                var ObservedUser = _db.FavouriteSeller.Where(O => O.SellerId == Id && O.UserID == userId).FirstOrDefault();
                _db.Remove(ObservedUser);
                _db.SaveChanges();
            }

            return Redirect("/ObservedSeller/Index");
        }

         public IActionResult Index()
        {

            var observedSellerVM = new ObservedSellerVM();

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            string userId = string.Empty;

            if (userName != null)
            {
                userId = _userManager.FindByNameAsync(userName).Result.Id;

            }
            if (userName != null)
            {
                var favouriteSellers = _db.FavouriteSeller
                    .Where(o => o.UserID == userId);                

                    var observedSeller = _db.Users
                    .ToList();


                observedSellerVM.ObservedSeller = observedSeller;
                observedSellerVM.FavouriteSeller = favouriteSellers.ToList();

                return View(observedSellerVM);
            }
            return NotFound();
    }
    }
}
