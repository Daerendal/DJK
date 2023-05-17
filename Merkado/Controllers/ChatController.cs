using DJK.DAL;
using DJK.Models;
using DJK.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DJK.Controllers
{
    public class ChatController : Controller
    {

        private readonly DJKDbContext _db;
        private UserManager<User> _userManager;

        public ChatController(DJKDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            try
            {
                string idValue = Request.Query["user"];
                // TODO: Trzeba jakoś wysłać userId jako ?user= by mógł się on wyświetlić na liście wiadomości, dzięki czemu będzie można wchodząc w kogoś profil wysyłać wiadomość
                // (będzie nas przekirowywało do czatu z tym użytkownikiem)

                var loggedUser = _userManager.GetUserAsync(User).Result;
                //var myMessageSendersID = _db.ChatMessages.Where(x => x.ToUserId == loggedUser.Id || x.FromUserId == loggedUser.Id || (idValue != null && (x.FromUserId == idValue && x.ToUserId == loggedUser.Id) || (x.FromUserId == loggedUser.Id && x.ToUserId == idValue))).Select(s => s.ToUserId == loggedUser.Id ? s.FromUserId : s.ToUserId).Where(k => !k.Equals(loggedUser.Id)).ToList();
                var myMessageSendersID = _db.Users.Select(x => x.Id).ToList();
                myMessageSendersID = myMessageSendersID.Distinct().ToList();

                var sendersDetails = new List<User>();

                foreach (var userID in myMessageSendersID)
                {
                    var user = _db.Users.Where(i => i.Id == userID).FirstOrDefault();

                    if (user != null)
                        sendersDetails.Add(user);
                }

                return View(sendersDetails);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }


        public IActionResult Messages(string id)
        {
            var loggedUser = _userManager.GetUserAsync(User).Result;

            var messagesFrom = _db.ChatMessages.Where(x => (x.FromUserId == id || x.ToUserId == id) && (x.FromUserId == loggedUser.Id || x.ToUserId == loggedUser.Id ) ).ToList();

            var allMessages = messagesFrom.OrderBy(x => x.CreatedDate).ToList();

            ChatVM chatVM = new ChatVM();
            chatVM.Messages = allMessages;
            chatVM.ToUser = id;
            if (allMessages != null)
            {
                return PartialView(chatVM);
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize]
        [Route("/Chat/SendMessage")]
        public IActionResult SendMessage(ChatVM chatVM)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(chatVM.NewMessege) && !string.IsNullOrEmpty(chatVM.ToUser))
                {
                    var message = new ChatMessage();
                    message.CreatedDate = DateTime.Now;
                    message.ToUserId = chatVM.ToUser;
                    message.Message = chatVM.NewMessege;
                    message.FromUserId = _userManager.GetUserAsync(User).Result.Id;

                    _db.ChatMessages.Add(message);
                    _db.SaveChanges();
                }
                return Json(chatVM);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }


    }
}
