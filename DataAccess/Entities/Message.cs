using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public int ConversationID { get; set; }

        [Required]
        public int SenderID { get; set; }

        public string MessageText { get; set; }

        public DateTime Timestamp { get; set; }

        public bool ReadStatus { get; set; }

        // Navigation properties
        [ForeignKey("ConversationID")]
        public virtual Conversation Conversation { get; set; }

        [ForeignKey("SenderID")]
        public virtual User Sender { get; set; }

        public virtual ICollection<Media> Media { get; set; }
    }

}
