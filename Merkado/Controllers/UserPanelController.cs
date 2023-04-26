using DJK.DAL;
using DJK.Models;
using DJK.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DJK.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly DJKDbContext _db;
        private UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserPanelController(DJKDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public IActionResult PurchaseHistory()
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;

            var userInfo = _db.Users
                              .Include(p => p.UserProducts.Where(s => s.IsSold == true))
                              .Where(i => i.Id == userId).FirstOrDefault(); 

            return View(userInfo);
        }

            public IActionResult Index()
        {
            var loggedUser = _userManager.GetUserAsync(User);
            
            if (loggedUser != null)
            {
                var user = _db.Users.Include(p => p.UserProducts)
                                        .ThenInclude(c => c.Category)
                                    .FirstOrDefault(u => u.Id == loggedUser.Result.Id);
                
                if (user != null && user.UserProducts != null)
                {
                    var products = user.UserProducts.Where(s => !s.IsSold).ToList();
                    return View(products);
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

        public IActionResult DeleteProduct(int productId)
        {
            var product = _db.Products.Include(i => i.Images).FirstOrDefault(x => x.ProductId == productId);
            var loggedUser = _userManager.GetUserAsync(User);
            var user = _db.Users.Include(p => p.UserProducts)
                        .ThenInclude(c => c.Category)
                    .FirstOrDefault(u => u.Id == loggedUser.Result.Id);

            if(user != null)
            {
                if (product != null && user.UserProducts.Contains(product))
                {
                    var productImages = product.Images;
                    _db.ProductImage.RemoveRange(productImages);
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                    return Redirect("/UserPanel/Index");
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

        public IActionResult RefreshProduct(int productId)
        {
            var product = _db.Products.FirstOrDefault(i => i.ProductId == productId);

            if (product != null)
            {
                product.AddedDate = DateTime.Now;
                _db.SaveChanges();
                return Redirect("/UserPanel/Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
