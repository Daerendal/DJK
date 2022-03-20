using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace Merkado.Controllers
{
    public class SearchController : Controller
    {

        private readonly ILogger<SearchController> _logger;
        private readonly MerkadoDbContext _db;

        public SearchController(ILogger<SearchController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }
      
        public async Task<IActionResult> Index(string searchString)
        {
            var searchResult = _db.Products
                                .Include(c => c.Category)
                                .Include(i => i.Images)
                                .Include(p => p.Providers)
                                .ToList();

            

            if (!String.IsNullOrEmpty(searchString))
            {
                searchResult = searchResult.Where(s => s.Name!.ToLower().Contains(searchString.Trim().ToLower())).ToList();
            }  

            return View(searchResult);
        }

    }
}
