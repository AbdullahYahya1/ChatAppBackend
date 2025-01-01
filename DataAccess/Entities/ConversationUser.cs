using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class ConversationUser
    {
        [Key]
        public int ConversationUserID { get; set; }

        [Required]
        public int ConversationID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime JoinedAt { get; set; }

        [ForeignKey("ConversationID")]
        public virtual Conversation Conversation { get; set; } 

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }

}
