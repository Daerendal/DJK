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

        public int countObserved(int item)
        {
            int count = _db.ObservedProducts.Count(o => o.ProductId == item);
            return count;
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

            string userId;

            if (item > 0)
            {
                

                var product = _db.Products
                                 .Include(i => i.Images)
                                 .Include(p => p.Providers)
                                 .Include(c => c.Category)                                 
                                 .Include(i => i.Images)
                                 .Where(id => id.ProductId == item)
                                 .FirstOrDefault();

                if (product != null)
                {
                    var similarProducts = _db.Products
                                             .Include(c => c.Category)
                                             .Where(p => p.Category == product.Category 
                                                       && (p.Price <= decimal.Multiply(product.Price,1.2m)
                                                       && p.ProductId != product.ProductId
                                                       && (p.Price >= decimal.Multiply(product.Price,0.8m)))
                                                    )
                                             .ToList();

                    productPageVM.SimilarProducts = similarProducts;
                }
                
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.Identity?.Name))
                {
                    userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
                    var ObservedProduct = _db.ObservedProducts.Any(O => O.ProductId == item && O.UserId == userId);

                    ViewBag.IfalreadyObserved = ObservedProduct;
                    ViewBag.hideHeart = false;
                }
               else
                {
                    ViewBag.IfalreadyObserved = false;
                    ViewBag.hideHeart = true;
                }
                var countobserved = countObserved(item);
                ViewBag.count = countobserved;
                
                var countVisit = product.Views;
                countVisit += 1;
                ViewBag.ViewsCount = countVisit;
                product.Views = countVisit;
                _db.Products.Update(product);
                _db.SaveChanges();
                productPageVM.CurrentProduct = product;
                ViewBag.andrzej = false;
                if (product != null)
                {
                    productPageVM.Seller = _db.Users.Where(id => id.UserProducts.Contains(product)).FirstOrDefault();
                    if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User.Identity?.Name))
                    {
                        userId = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result.Id;
                        if (userId == productPageVM.Seller.Id)
                        {
                            ViewBag.andrzej = true;
                        }
                    }



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
