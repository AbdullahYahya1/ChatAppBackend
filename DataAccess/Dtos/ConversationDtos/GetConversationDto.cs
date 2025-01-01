using Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dtos.MessageDtos;
using DataAccess.Dtos.ConversationUserDtos;

namespace DataAccess.Dtos.ConversationDtos
{
    public class GetConversationDto
    {
        public int ConversationID { get; set; }
        public ConversationType ConversationType { get; set; }
        public string? ConversationName { get; set; }
        public virtual GetMessageDto? LastMessage { get; set; }
        public virtual ICollection<GetConversationUserDto> ConversationUsers { get; set; }
    }
}
