using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Dto.Message
{
    public class MessagesForReturnSended
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string To { get; set; }
    }
}
