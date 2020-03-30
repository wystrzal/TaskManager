using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.MessageRepo
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetSendedMessages(int senderId);
        Task<IEnumerable<Message>> GetReceivedMessages(int recipientId);
    }
}
