using DJK.DAL;
using DJK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace DJK.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DJKDbContext _db;

        public HomeController(ILogger<HomeController> logger, DJKDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var productList = _db.Products
                                .Include(c => c.Category)
                                .Include(i => i.Images)
                                .Include(p => p.Providers)
                                .Where(s => !s.IsSold)
                                .ToList();

            return View(productList);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Policy()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}