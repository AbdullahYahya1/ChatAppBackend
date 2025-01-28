using DataAccess.Dtos.General;
using DataAccess.Dtos.MessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IMessageService
    {
        Task<ResponseModel<GetMessageDto>> PostMessage(PostMessageDto postMessageDto);
        Task<ResponseModel<ICollection<GetMessageDto>>> GetMessages(int ConversationID,int? lastMessageId=null);

    }
}
