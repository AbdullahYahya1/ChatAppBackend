using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.MessageDtos
{
    public class PostMessageDto
    {
        public int ConversationID { get; set; }
        public string MessageText { get; set; }
    }
}
