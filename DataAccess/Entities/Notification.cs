using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int MessageID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("MessageID")]
        public virtual Message Message { get; set; }
    }

}
