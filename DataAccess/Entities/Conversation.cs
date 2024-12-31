using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; } // "group" or "chat"

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

        public int? LastMessageID { get; set; }
        // Navigation properties
        [ForeignKey("LastMessageID")]
        public virtual Message LastMessage { get; set; }

        public virtual ICollection<ConversationUser> ConversationUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }

}
