using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data.Repository.MessageRepo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext dataContext;

        public MessageRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<Message>> GetReceivedMessages(int recipientId)
        {
            return await dataContext.Messages.Include(m => m.Sender).Where(m => m.RecipientId == recipientId).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetSendedMessages(int senderId)
        {
            return await dataContext.Messages.Include(m => m.Recipient).Where(m => m.SenderId == senderId).ToListAsync();
        }
    }
}
