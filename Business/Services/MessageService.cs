using Business.Entities;
using Business.FileService;
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
    public class MessageService : IMessageService
    {
        private readonly IServicesDependency<Message> _dep;
        private readonly FileService.FileService _fileService;

        public MessageService(IServicesDependency<Message> dep , FileService.FileService fileService)
        {
            _fileService = fileService;
            _dep = dep;
        }

    public async Task<ResponseModel<GetMessageDto>> PostMessage(PostMessageDto postMessageDto)
        {
            var currentUserId = _dep.GetUserId();
            var convCheck = await _dep.UnitOfWork.Conversations
                .CheckUserAllowToSendToConversation(currentUserId, postMessageDto.ConversationID);
            if (!convCheck)
            {
                return new ResponseModel<GetMessageDto>
                {
                    IsSuccess = false,
                    Message = "NotAuthorized"
                };
            }
            var message = new Message
            {
                MessageText = postMessageDto.MessageText,
                ConversationID = postMessageDto.ConversationID,
                SenderID = currentUserId,
                ReadStatus = false,
                Timestamp = DateTime.UtcNow,
            };
            await _dep.UnitOfWork.Messages.AddAsync(message);
            await _dep.UnitOfWork.SaveChangesAsync();
            if (postMessageDto.Files != null && postMessageDto.Files.Count > 0)
            {
                foreach (var file in postMessageDto.Files)
                {
                    if (file?.Length > 0)
                    {
                        var mediaType = GetMediaType(file.ContentType); 

                        var url = _fileService.SaveFile(file, mediaType);
                        var media = new Media
                        {
                            MessageID = message.MessageID,
                            MediaType = mediaType, 
                            URL = url,
                            UploadedAt = DateTime.UtcNow
                        };
                        await _dep.UnitOfWork.Media.AddAsync(media);
                    }
                }
            }
            var conversation = await _dep.UnitOfWork.Conversations
            .GetByIdAsync(postMessageDto.ConversationID);
        conversation.LastMessage = message;
        conversation.LastUpdate = DateTime.UtcNow;
        await _dep.UnitOfWork.SaveChangesAsync();
            return new ResponseModel<GetMessageDto>
            {
                Result = _dep.Mapper.Map<GetMessageDto>(message),
                IsSuccess = true
            };
    }
     private MediaType GetMediaType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => MediaType.Images,
                "image/jfif" => MediaType.Images,
                "image/png" => MediaType.Images,
                "image/gif" => MediaType.Images,
                "application/pdf" => MediaType.Documents,
                "application/msword" => MediaType.Documents,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => MediaType.Documents,
                "video/mp4" => MediaType.Videos,
                "audio/mpeg" => MediaType.Audio,
                "webm" => MediaType.Audio,
                "application/zip" => MediaType.Archives,
                _ => MediaType.Other // Default to 'Other' if not recognized
            };
        }

        public async Task<ResponseModel<ICollection<GetMessageDto>>> GetMessages(int ConversationID, int? lastMessageId)
        {
            var currentUserId = _dep.GetUserId();
            var conv = await _dep.UnitOfWork.Conversations.CheckUserAllowToSendToConversation(currentUserId, ConversationID);
            if (!conv)
            {
                return new ResponseModel<ICollection<GetMessageDto>>
                {
                    IsSuccess = false,
                    Message = "NotAuthorized"
                };
            }
            await _dep.UnitOfWork.Messages.UpdateMessagesReadStatus(ConversationID, currentUserId);

            var messages =await _dep.UnitOfWork.Messages.GetMessaegsByConversationID(ConversationID , currentUserId,lastMessageId);
            return new ResponseModel<ICollection<GetMessageDto>>
            {
                IsSuccess = true,
                Result = _dep.Mapper.Map<ICollection<GetMessageDto>>(messages)
            };
        }
    }
}