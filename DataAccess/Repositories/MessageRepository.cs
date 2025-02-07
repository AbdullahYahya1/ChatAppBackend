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


        public async Task<ICollection<Message>> GetMessaegsByConversationID(int conversationID, int currentUserI, int? lastMessageId = null, int pageSize = 20)
        {
            var query = _context.Messages.Include(M=>M.Media)
                .Where(m => m.ConversationID == conversationID)
                .OrderByDescending(m => m.MessageID) 
                .AsNoTracking();
            if (lastMessageId.HasValue)
            {
                query = query.Where(m => m.MessageID < lastMessageId.Value);
            }
            var messages = await query.Take(pageSize).ToListAsync();
            return messages.OrderBy(m => m.MessageID).ToList();
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
