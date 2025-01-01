using Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dtos.General;

namespace DataAccess.Dtos.ConversationUserDtos
{
    public class GetConversationUserDto
    {
        public DateTime JoinedAt { get; set; }
        public GetUserDto User { get; set; }
    }
}
