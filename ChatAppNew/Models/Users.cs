using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ChatAppNew.Models
{
    public class Users:IdentityUser
    {
        public string Name { get; set; }

        public ICollection<ChatUser> chats { get; set; }
    }
}
