using Business.Entities;
using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.MessageDtos;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MessageService:IMessageService
    {
        private readonly IServicesDependency<Message> _dep;
        public MessageService(IServicesDependency<Message> dep)
        {
            _dep = dep;
        }
        public async Task<ResponseModel> PostMessage(PostMessageDto postMessageDto){

            var currentUserId = _dep.GetUserId();
            var conv = await _dep.UnitOfWork.Conversations.CheckUserAllowToSendToConversation(currentUserId, postMessageDto.ConversationID);

            if (!conv) 
            {
                return new ResponseModel()
                {
                    IsSuccess = false,
                    Message = "NotAuthorized" 
                };
            }
            var message = new Message()
            {
                MessageText = postMessageDto.MessageText,
                ConversationID = postMessageDto.ConversationID,
                SenderID = currentUserId,
                ReadStatus = false,
                Timestamp = DateTime.UtcNow,
            };
            await _dep.UnitOfWork.Messages.AddAsync(message);
            await _dep.UnitOfWork.SaveChangesAsync();
            return new ResponseModel()
            {
                IsSuccess = true,
            }; 
        }
    }
}