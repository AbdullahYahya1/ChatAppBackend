using Business.Entities;
using DataAccess.Dtos.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.ConversationDtos
{
    public class ConversationDto
    {
        public int ConversationID { get; set; }
        public DateTime LastUpdate { get; set; }
        public ConversationType ConversationType { get; set; }
        public Message LastMessage { get; set; } 
        public GetUserDto FirstUser { get; set; }
    }
}
