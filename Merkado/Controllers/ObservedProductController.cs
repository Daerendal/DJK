using DJK.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DJK.ViewModels;
using Microsoft.AspNetCore.Identity;
using DJK.Models;
using Microsoft.AspNetCore.Authorization;

namespace DJK.Controllers
{
    public class ObservedProductController : Controller
    {
        private readonly ILogger<ObservedProductController> _logger;
        private readonly DJKDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ObservedProductController(ILogger<ObservedProductController> logger, DJKDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {

            var ObservedPageVM = new ObservedPageVM();

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            string userId = string.Empty;
            
            if (userName != null)
            {
                userId = _userManager.FindByNameAsync(userName).Result.Id;

            }
            if (userName != null)
            {
                var ObservedProducts = _db.ObservedProducts
                    .Where(o => o.UserId == userId)
                    .ToList();


                var productList = _db.Products
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(p => p.Providers)
                .Where(s => s.IsSold == false)
                .ToList();
                  
               


                ObservedPageVM.OProducts = productList;
                ObservedPageVM.ObservedProduct = ObservedProducts;

                return View(ObservedPageVM);
            }
            return NotFound();
        }
        public IActionResult DeleteObservedProduct(int productId)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
            var ifObservedProduct = _db.ObservedProducts.Where(O => O.ProductId == productId && O.UserId == userId).FirstOrDefault();
            if (userId != null)
            {
                    _db.ObservedProducts.Remove(ifObservedProduct);
                    _db.SaveChanges();
                    return Redirect("/ObservedProduct/Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
