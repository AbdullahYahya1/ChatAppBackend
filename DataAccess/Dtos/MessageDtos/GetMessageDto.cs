using Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dtos.MediaDtos;

namespace DataAccess.Dtos.MessageDtos
{
    public class GetMessageDto
    {
        public int MessageID { get; set; }
        public int SenderID { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
        public bool ReadStatus { get; set; }
        public virtual ICollection<GetMediaDto> Media { get; set; }
    }
}
