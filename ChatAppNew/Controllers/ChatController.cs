using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatAppNew.Data;
using ChatAppNew.Hubs;
using ChatAppNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppNew.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public ChatController(IHubContext<ChatHub> hubContext,ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("/Chat/JoinRoom/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom( string connectionId,  string roomId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, roomId);
            return Ok();
        }
        [HttpPost("/Chat/SendMessage")]
        public async Task<IActionResult> SendMessage(string message,int roomId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            var Reciever = _context.ChatUsers.FirstOrDefault(x => x.ChatId == roomId && x.UserId != userId);
            var userName = user.Name;
            var msg = new Message
            {
                Name = userName,
                Text = message,
                newDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                TimeStamp =DateTime.Parse (DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")),
                ChatId = roomId,
                UserId = userId,
                RecieverId=Reciever.UserId
                

            };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", msg);
            await _hubContext.Clients.All.SendAsync("hello");
            return Ok();
        }
    }
}
