using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;

namespace Merkado.Controllers
{
    public class ObservedProductController : Controller
    {
        private readonly ILogger<ObservedProductController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ObservedProductController(ILogger<ObservedProductController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
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
                    .Where(o => o.UserId == userId);


                var productList = _db.Products
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(p => p.Providers)
                .ToList();

                ObservedPageVM.OProducts = productList;
                ObservedPageVM.ObservedProduct = ObservedProducts.ToList();

                return View(ObservedPageVM);
            }
            return NotFound();
        }
    }
}
