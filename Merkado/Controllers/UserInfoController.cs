using Merkado.DAL;
using Merkado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Merkado.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Merkado.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly MerkadoDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public static string? currentUser { get; set; }

        public UserInfoController(MerkadoDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

       

        public IActionResult Index(string user)
        {
            if (user != null)
            {
                currentUser = user;
            }

            if(currentUser != null)
            {


                var userInfo = _db.Users
                              .Include(p => p.UserProducts)
                              .Include(o => o.Opinions)
                              .Where(i => i.Id == currentUser)
                              .FirstOrDefault();

                var opinions = userInfo.Opinions;

                if (opinions != null && opinions.Count > 0)
                {
                    foreach (Opinion opinion in opinions)
                    {
                        opinion.ReviewerName = _db.Users.Where(rev => rev.Id == opinion.ReviewerId).Select(n => n.FirstName).FirstOrDefault();
                        
                    }

                    userInfo.Opinions = opinions;
                }
               
                return View(userInfo);
            }
            else
            {
                return View("ErrorPage");
            }  
        }


        public async Task<IActionResult> AddOpinion(string comment, int rating)
        {
            var lastOpinion = _db.Opinions.Include(x => x.OpinionId).OrderByDescending().Take(1);
            var logedUser = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext?.User.Identity?.Name).Result;
            
            if (rating == null)
            {
                rating = 0;
            }

            var opinion = new Opinion()
            {
                OpinionId = 103,
                Comment = comment,
                Rate = rating,
                ReviewerId = logedUser.Id,
                ReviewerName = logedUser.FirstName + " " + logedUser.LastName
            };

            _db.Opinions.Add(opinion);
            await _db.SaveChangesAsync();

            return Redirect("Index");
        }



    }
}
