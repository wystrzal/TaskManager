using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Message
{
    public class MessageForReturnReceivedMessages
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string From { get; set; }
    }
}
