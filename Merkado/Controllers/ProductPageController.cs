using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;

namespace Merkado.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly ILogger<ProductPageController> _logger;
        private readonly MerkadoDbContext _db;


        public ProductPageController(ILogger<ProductPageController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        public IActionResult Index(int item)
        {
            ViewBag.CurrentItem = item;

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


                productPageVM.CurrentProduct = product;

                if (product != null)
                {
                    productPageVM.Seller = _db.Users.Where(id => id.UserProducts.Contains(product)).FirstOrDefault();
                }
                
                
                return View(productPageVM);
            }
            else
            {
                return View("ErrorPage");
            }
        } 
    }
}
