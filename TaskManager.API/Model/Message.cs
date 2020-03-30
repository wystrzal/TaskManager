using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Model
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
    }
}
