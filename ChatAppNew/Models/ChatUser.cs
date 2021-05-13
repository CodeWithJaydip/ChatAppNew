using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppNew.Models
{
    public class ChatUser
    {
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; }
    }
}
