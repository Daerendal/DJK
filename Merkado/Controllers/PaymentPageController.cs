using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;

namespace Merkado.Controllers
{
    public class PaymentPageController : Controller
    {
        private readonly ILogger<PaymentPageController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PaymentPageController(ILogger<PaymentPageController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
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
