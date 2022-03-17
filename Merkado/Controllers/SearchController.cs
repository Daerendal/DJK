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

        private readonly ILogger<HomeController> _logger;
        private readonly MerkadoDbContext _db;

        public SearchController(ILogger<HomeController> logger, MerkadoDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
