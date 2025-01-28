using Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.MediaDtos
{
    public class GetMediaDto
    {
        public MediaType MediaType { get; set; }
        public string URL { get; set; }
    }
}
