using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class Media
    {
        [Key]
        public int MediaID { get; set; }

        [Required]
        public int MessageID { get; set; }

        [Required]
        [MaxLength(50)]
        public string MediaType { get; set; }

        [Required]
        public string URL { get; set; }

        public DateTime UploadedAt { get; set; }

        [ForeignKey("MessageID")]
        public virtual Message Message { get; set; }
    }

}
