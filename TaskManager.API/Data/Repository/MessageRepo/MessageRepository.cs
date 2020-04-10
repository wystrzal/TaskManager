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

        public async Task<Message> GetMessage(int id)
        {
            return await dataContext.Messages.Include(m => m.Sender).Include(m => m.Recipient)
                .Where(m => m.MessageId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Message>> GetReceivedMessages(int recipientId, int skip)
        {
            return await dataContext.Messages.Include(m => m.Sender)
                .Where(m => m.RecipientId == recipientId && m.RecipientDeleted != true)
                .Skip(skip).Take(15).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetSendedMessages(int senderId, int skip)
        {
            return await dataContext.Messages.Include(m => m.Recipient)
                .Where(m => m.SenderId == senderId && m.SenderDeleted != true)
                .Skip(skip).Take(15).ToListAsync();
        }
    }
}
