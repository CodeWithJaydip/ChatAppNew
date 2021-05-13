using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppNew.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public string newDate { get; set; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public Chat Chat { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; }

        public string RecieverId { get; set; }
    }
}
