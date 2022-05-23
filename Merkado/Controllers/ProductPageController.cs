using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;

namespace Merkado.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly ILogger<ProductPageController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProductPageController(ILogger<ProductPageController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
        public void addtoFavourite(int productID)
        {

            var userId= _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
            
            var ifObservedProduct = _db.ObservedProducts.Any(O => O.ProductId == productID && O.UserId == userId);
            if (ifObservedProduct)
            {
                removefavourite(productID);
            }
            else
            {
                var ObservedProduct = new ObservedProduct();
                ObservedProduct.ProductId = productID;
                ObservedProduct.UserId = userId;
                _db.Add(ObservedProduct);
                _db.SaveChanges();
            }
        }

        public void removefavourite(int  productID)
        {
            var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
            var ifObservedProduct = _db.ObservedProducts.Any(O => O.ProductId == productID && O.UserId == userId);
            if (ifObservedProduct)
            {
                var ObservedProduct = _db.ObservedProducts.Where(O => O.ProductId == productID && O.UserId == userId).FirstOrDefault();
                _db.Remove(ObservedProduct);
                _db.SaveChanges();
            }
            else {
                addtoFavourite(productID);
            }
        }
        public IActionResult Index(int item)
        {
            
           
            var productPageVM = new ProductPageVM();



            if (item > 0)
            {
                

                var product = _db.Products
                                 .Include(i => i.Images)
                                 .Include(p => p.Providers)
                                 .Include(c => c.Category)                                 
                                 .Include(i => i.Images)
                                 .Where(id => id.ProductId == item)
                                 .FirstOrDefault();
                if(!String.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.Identity?.Name))
                {
                    var userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
                    var ObservedProduct = _db.ObservedProducts.Any(O => O.ProductId == item && O.UserId == userId);

                    ViewBag.IfalreadyObserved = ObservedProduct;
                    ViewBag.hideHeart = false;
                }
               else
                {
                    ViewBag.IfalreadyObserved = false;
                    ViewBag.hideHeart = true;
                }


                productPageVM.CurrentProduct = product;

                if (product != null)
                {
                    productPageVM.Seller = _db.Users.Where(id => id.UserProducts.Contains(product)).FirstOrDefault();

                    

                    return View(productPageVM);
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
    }
}
