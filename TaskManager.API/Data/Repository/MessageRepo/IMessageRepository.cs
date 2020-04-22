using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.MessageRepo
{
    public interface IMessageRepository : IMainRepository
    {
        Task<IEnumerable<Message>> GetSendedMessages(int senderId, int skip);
        Task<IEnumerable<Message>> GetReceivedMessages(int recipientId, int skip);
        Task<Message> GetMessage(int id);
    }
}
