using Merkado.DAL;
using Merkado.Models;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Merkado.Controllers
{
    public class AddProductController : Controller
    {
        private readonly ILogger<AddProductController> _logger;
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public AddProductController(ILogger<AddProductController> logger, MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            AddProductVM addProductVM = new AddProductVM();
            addProductVM.Products = _db.Products.ToList();
            addProductVM.Categories = _db.Categories.ToList();
            addProductVM.Providers = _db.Providers.ToList();
            addProductVM.Seller = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            return View(addProductVM);
        }
    }
}
