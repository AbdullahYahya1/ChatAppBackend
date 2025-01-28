using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public enum MediaType
    {
        Images = 0,        // Includes JPG, PNG, GIF, etc.
        Documents = 1,     // Includes PDF, DOCX, XLSX, etc.
        Videos = 2,        // Includes MP4, AVI, etc.
        Audio = 3,         // Includes MP3, WAV, etc.
        Archives = 4,      // Includes ZIP, RAR, etc.
        Other = 5          // Any other file types
    }

    public class Media
    {
        [Key]
        public int MediaID { get; set; }

        [Required]
        public int MessageID { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

        [Required]
        public string URL { get; set; }

        public DateTime UploadedAt { get; set; }

        [ForeignKey("MessageID")]
        public virtual Message Message { get; set; }
    }

}
