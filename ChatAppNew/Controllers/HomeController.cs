using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatAppNew.Data;
using ChatAppNew.Models;
using ChatAppNew.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatAppNew.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]

        public IActionResult Find()
        {
            var users = _context.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();
            return View(users);


        }

        [Authorize]

        public IActionResult Private()
        {
            var chats = _context.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.Users)
                .Where(x => x.Type == ChatType.Private &&
                   x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();
            return View(chats);

        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var count = 0;
            var alreadyInFriend = 0;
            var reciverChatUsers = _context.ChatUsers.Where(x => x.UserId == userId).ToList();

            var recieverUser = _context.Users.FirstOrDefault(x => x.Id == userId);


            var LoginUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var senderChatUsers = _context.ChatUsers.Where(x => x.UserId == LoginUserId).ToList();
            foreach (var reciverChatuser in reciverChatUsers)
            {
                var recieverChatId = reciverChatuser.ChatId;
                foreach (var senderChatUser in senderChatUsers)
                {
                    var senderChatId = senderChatUser.ChatId;
                    if (recieverChatId == senderChatId)
                    {
                        count = recieverChatId;
                        break;
                    }
                }
                if (count != 0)
                {
                    alreadyInFriend = recieverChatId;
                    break;
                }

            }
            if (alreadyInFriend != 0)
            {
                return RedirectToAction("Chat", new { id = alreadyInFriend });

            }
            else
            {
                var chat = new Chat
                {
                    Type = ChatType.Private,
                    Name = null
                };
                chat.Users.Add(new ChatUser
                {
                    UserId = userId
                });
                chat.Users.Add(new ChatUser
                {
                    UserId = LoginUserId
                });
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Chat", new { id = chat.Id });
            }







        }

        //[Authorize]

        //public IActionResult Chat(int? id)
        //{
        //    ViewBag.numbers = ConnectedUser.Ids.Count();
        //    ChatViewModel model = new ChatViewModel();
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;




        //    var Privatechats = _context.Chats
        //      .Include(x => x.Users)
        //      .ThenInclude(x => x.Users)
        //      .Where(x => x.Type == ChatType.Private &&
        //         x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //      .ToList();
        //    var users = _context.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();


        //    if (id == null)
        //    {
        //        model.Chats = Privatechats;
        //        model.Chat = null;
        //        model.Users = users;
        //    }
        //    else
        //    {
        //        var Chats = _context.ChatUsers.Include(y => y.Users).Where(x => x.ChatId == id && x.UserId != userId).ToList();
        //        ViewBag.chatId = Chats[0].ChatId;
        //        var recieverUserId = Chats[0].UserId;
        //        var reciever = _context.Users.FirstOrDefault(x => x.Id == recieverUserId);
        //        var recieverName = reciever.Name;

        //        ViewBag.reciverName = recieverName;


        //        var chats = _context.Chats.Include(x => x.Messages).Include(y => y.Users).FirstOrDefault(x => x.Id == id);
        //        model.Chat = chats;
        //        model.Chats = Privatechats;
        //        model.Users = users;
        //    }
        //    //return View(chats);
        //    return View(model);
        //}


        [Authorize]

        public IActionResult Chat(int? id)
        {
            ViewBag.numbers = ConnectedUser.Ids.Count();
            ChatViewModel model = new ChatViewModel();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;




            var Privatechats = _context.Chats
              .Include(x => x.Users)
              .ThenInclude(x => x.Users)
              .Where(x => x.Type == ChatType.Private &&
                 x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
              .ToList();
            var users = _context.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();


            if (id == null)
            {
                model.Chats = Privatechats;
                model.Chat = null;
                model.Users = users;
            }
            else
            {
                var Chats = _context.ChatUsers.Include(y => y.Users).Where(x => x.ChatId == id && x.UserId != userId).ToList();
                ViewBag.chatId = Chats[0].ChatId;
                var recieverUserId = Chats[0].UserId;
                var reciever = _context.Users.FirstOrDefault(x => x.Id == recieverUserId);
                var recieverName = reciever.Name;

                ViewBag.reciverName = recieverName;


                var chats = _context.Chats.Include(x => x.Messages).Include(y => y.Users).FirstOrDefault(x => x.Id == id);
                model.Chat = chats;
                model.Chats = Privatechats;
                model.Users = users;
            }
           
                return View(model);
            
            
            
             
            
           

        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            var userName = user.Name;
            var msg = new Message
            {
                Name = userName,
                Text = message,
                TimeStamp = DateTime.Now,
                ChatId = chatId,
                UserId = userId

            };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();
            return RedirectToAction("Chat", new { id = chatId });
        }

        [HttpPost]
        public JsonResult GetUser(string name)
       {
            //var users = _context.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();
            //var IUser = users.AsEnumerable();
           
            //if (!String.IsNullOrEmpty(name))
            //{


                var user = _context.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value && x.Name.Contains(name)).ToList();
                return Json(user);
            //}
            
            
        }

        public IActionResult Privacy()
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
