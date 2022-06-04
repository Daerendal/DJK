using Merkado.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Merkado.Controllers
{
    public class ChatController : Controller
    {
        private readonly MerkadoDbContext _db;

        public ChatController(MerkadoDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
