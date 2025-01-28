using Business.Entities;
using DataAccess.Dtos.ConversationDtos;
using DataAccess.Dtos.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IConversationRepository:IRepository<Conversation>
    {
        Task<ICollection<ConversationDto>> GetAllConversationsByUserId(int userId, Pagination pagination, string? email);
        Task<bool> CheckUserAllowToSendToConversation(int userId,  int ConversationID);

    }
}
