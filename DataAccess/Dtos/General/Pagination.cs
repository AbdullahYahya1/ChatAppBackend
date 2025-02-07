using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.General
{
    public class Pagination
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 100;
    }
}
