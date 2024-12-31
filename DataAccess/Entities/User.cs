using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? RefreshToken { get; set; }
        public UserType UserType { get; set; }
        public DateTime? LastTimeConnected { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool OnlineStatus { get; set; }

        public virtual ICollection<ConversationUser> ConversationUsers { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }

}
