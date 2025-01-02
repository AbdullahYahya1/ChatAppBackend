using Business.Context;
using Business.Entities;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly ChatDpContext _context;
        public MessageRepository(ChatDpContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Message>> GetMessaegsByConversationID(int conversationID, int currentUserId)
        {
            return await _context.Messages
                .Where(m => m.ConversationID == conversationID)
                .ToListAsync();
        }
        public async Task UpdateMessagesReadStatus(int conversationID, int currentUserId)
        {
            await _context.Messages
            .Where(m => m.ConversationID == conversationID &&
                        m.SenderID != currentUserId &&
                        !m.ReadStatus)
            .ExecuteUpdateAsync(m => m.SetProperty(x => x.ReadStatus, true));
        }
    }
}
