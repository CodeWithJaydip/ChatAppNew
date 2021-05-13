using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppNew.Models;

namespace ChatAppNew.ViewModels
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            Chats = new List<Chat>();
            Users = new List<Users>();
        }
        public Chat Chat { get; set; }
        public List<Chat> Chats { get; set; }
        public List<Users> Users { get; set; }
    }
}
