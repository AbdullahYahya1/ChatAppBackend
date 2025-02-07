using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.IServices
{
    
    public interface IConversationServices
    {
        Task<ResponseModel> AddChat(EmailDto emailDto);
        Task<ResponseModel<ICollection<ConversationDto>>> GetCurrentConversations(Pagination pagination, string? email);
        Task<ResponseModel> UpdateReadMessages(int ConversationId);
    }
}