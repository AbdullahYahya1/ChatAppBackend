using Business.Context;
using Business.Entities;
using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.General;
using DataAccess.IRepositories;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ICollection<ConversationDto>> GetAllConversationsByUserId(int userId, Pagination pagination, string? email)
        {
            var query = _context.Conversations
            .Where(c => c.ConversationUsers.Any(cu => cu.UserID == userId))
            .OrderByDescending(c => c.LastUpdate);

            if (!string.IsNullOrEmpty(email))
            {
                query = (IOrderedQueryable<Conversation>)query.Where(c => c.ConversationUsers
                    .Any(cu => cu.User != null && !string.IsNullOrEmpty(cu.User.Email) && cu.User.Email.Contains(email)));
            }
            var conversations = await query
                .Skip((pagination.page - 1) * pagination.pageSize)
                .Take(pagination.pageSize)
                .Select(c => new ConversationDto
                {
                    ConversationID = c.ConversationID,
                    LastUpdate = c.LastUpdate,
                    LastMessage = c.LastMessage,
                    ConversationType = c.ConversationType,
                    NumOfNewMessages = c.Messages.Count(m => m.ReadStatus == false && m.SenderID != userId),
                    FirstUser = c.ConversationUsers
                        .Where(cu => cu.UserID != userId)
                        .Select(cu => new GetUserDto
                        {
                            Email = cu.User.Email,
                            ProfileImg=cu.User.ProfileImg
                        })
                        .FirstOrDefault()
                }).AsNoTracking()
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

        public async Task<List<string>> GetAllEmails(int userId, int ConversationID)
        {
            var Emails = await _context.ConversationUsers.Where(c => c.UserID != userId && c.ConversationID == ConversationID).Select(c => c.User.Email).ToListAsync();
            return Emails; 
        }
        public async Task ReadStatusConversation(int currentUserId, int conversationId)
        {
            var messages= await _context.Messages.Where(C => C.ConversationID == conversationId && C.ReadStatus ==false).ToListAsync();
            foreach (var message in messages)
            {
                message.ReadStatus = true;
            }
            await _context.SaveChangesAsync(); 
        }
    }
}