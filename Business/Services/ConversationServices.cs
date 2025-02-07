using Business.Entities;
using Business.Hubs;
using Business.IServices;
using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ConversationServices: IConversationServices
    {
        private readonly IServicesDependency<Conversation> _dep;
        private readonly IHubContext<UserHub> _hubContext;

        public ConversationServices(IServicesDependency<Conversation> dep , IHubContext<UserHub> hubContext) {
            _dep = dep;
            _hubContext = hubContext;
        }
        
        public async Task<ResponseModel> AddChat(EmailDto emailDto)
        {
            var currntUserId = _dep.GetUserId();
            var userByEmail = await _dep.UnitOfWork.Users.GetUserByEmail(emailDto.Email);

            if (userByEmail == null)
            {
                return new ResponseModel() { IsSuccess = false, Message = "NotValidEmail" };
            }
            if (currntUserId == -1)
            {
                return new ResponseModel() { IsSuccess = false, Message = "NotAuthincated" };
            }
            if(userByEmail.UserID == currntUserId)
            {
                return new ResponseModel() { IsSuccess = false, Message = "YouCanNotAddYourSelf" }; 
            }
            var existingConversation = await _dep.UnitOfWork.Conversations
            .FindAsync(c =>
                c.ConversationType == ConversationType.chat &&
                c.ConversationUsers.Any(cu => cu.UserID == currntUserId) &&
                c.ConversationUsers.Any(cu => cu.UserID == userByEmail.UserID)
            );
            if (existingConversation != null)
            {
                return new ResponseModel() { IsSuccess = false, Message = "ConversationAlreadyExists" };
            }
            var Conversation = new Conversation()
            {
                ConversationType = ConversationType.chat,
                CreatedAt = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                ConversationUsers = new List<ConversationUser>
                {
                    new ConversationUser { UserID = currntUserId , JoinedAt = DateTime.UtcNow},
                    new ConversationUser { UserID = userByEmail.UserID , JoinedAt= DateTime.UtcNow}
                }
            };
            await _dep.UnitOfWork.Conversations.AddAsync(Conversation);
            await _dep.UnitOfWork.SaveChangesAsync();
            await _hubContext.Clients.User(userByEmail.Email).SendAsync("ReceiveFriendRequest", _dep.GetUserEmail());
            return new ResponseModel(){ IsSuccess = true };
        }
        public async Task<ResponseModel<ICollection<ConversationDto>>> GetCurrentConversations(Pagination pagination , string? email)
        {
            var currentUserId = _dep.GetUserId();
            var conversations =await _dep.UnitOfWork.Conversations.GetAllConversationsByUserId(currentUserId, pagination, email);
            return new ResponseModel<ICollection<ConversationDto>> { Result= conversations, IsSuccess=true };
        }

        public async Task<ResponseModel> UpdateReadMessages(int ConversationId)
        {
            var currentUserId = _dep.GetUserId();
            await _dep.UnitOfWork.Conversations.ReadStatusConversation(currentUserId, ConversationId);
            return new ResponseModel() { IsSuccess = true };
        }
    }
}
