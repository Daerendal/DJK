using DJK.DAL;
using DJK.Models;
using DJK.ViewModels;
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
                var loggedUser = _userManager.GetUserAsync(User).Result;
                var myMessageSendersID = _db.ChatMessages.Where(x => x.ToUserId == loggedUser.Id).Select(s => s.FromUserId).ToList();
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

            var messagesFrom = _db.ChatMessages.Where(x => x.FromUserId == id && x.ToUserId == loggedUser.Id).ToList();
            var messagesTo = _db.ChatMessages.Where(x => x.ToUserId == id && x.FromUserId == loggedUser.Id).ToList();

            messagesFrom.AddRange(messagesTo);

            var allMessages = messagesFrom.OrderBy(x => x.CreatedDate).ToList();

            ChatVM chatVM = new ChatVM();
            chatVM.Messages = allMessages;

            if (allMessages != null)
            {
                return PartialView(chatVM);
            }
            else
            {
                return NotFound();
            }
        }

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
                return Redirect("Index");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }


    }
}
