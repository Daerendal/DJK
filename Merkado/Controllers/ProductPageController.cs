using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Merkado.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly ILogger<ProductPageController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;


        public ProductPageController(ILogger<ProductPageController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        public IActionResult Index(string item)
        {
            ViewBag.CurrentItem = item;
            
            var productList = _db.Products
                                .Include(c => c.Category)
                                .Include(i => i.Images)
                                .Include(p => p.Providers)
                                .ToList();
            var categoryListy = _db.Products
                                .Include(c => c.Category)
                                .ToList();
            var providerList = _db.Products
                                .Include(p => p.Providers)
                                .ToList();

            

            if (!String.IsNullOrEmpty(item))
            {
                productList = productList.Where(s => s.Name!.ToLower().Contains(item.Trim().ToLower())).ToList();
            }

            return View(productList);
        }

       
    }
}
