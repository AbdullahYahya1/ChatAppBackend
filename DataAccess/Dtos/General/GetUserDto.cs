﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.General
{
    public class GetUserDto
    {
        public int UserID { get; set; }
        public string? ProfileImg { get; set; }
        public string? Email { get; set; }
    }
}
