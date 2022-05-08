using Microsoft.AspNetCore.Mvc;

namespace Merkado.Controllers
{
    public class UserInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
