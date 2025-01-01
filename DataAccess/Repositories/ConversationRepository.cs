using Business.Context;
using Business.Entities;
using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.General;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        private readonly ChatDpContext _context;
        public ConversationRepository(ChatDpContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ICollection<ConversationDto>> GetAllConversationsByUserId(int userId, Pagination pagination)
        {
            var conversations = await _context.Conversations
                .Where(c => c.ConversationUsers.Any(cu => cu.UserID == userId))
                .OrderByDescending(c => c.LastUpdate)
                .Skip((pagination.page - 1) * pagination.pageSize)
                .Take(pagination.pageSize)
                .Select(c => new ConversationDto
                {
                    ConversationID = c.ConversationID,
                    LastUpdate = c.LastUpdate,
                    LastMessage = c.LastMessage,
                    ConversationType=c.ConversationType,
                    FirstUser = c.ConversationUsers
                        .Where(cu => cu.UserID != userId) 
                        .Select(cu => new GetUserDto
                        {
                            Email = cu.User.Email
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();
            return conversations;
        }
        public async Task<bool> CheckUserAllowToSendToConversation(int userId, int ConversationID)
        {
            var val = await _context.Conversations
                .AnyAsync(c => c.ConversationID == ConversationID &&
                               c.ConversationUsers.Any(u => u.UserID == userId));
            return val;
        }
    }
}
