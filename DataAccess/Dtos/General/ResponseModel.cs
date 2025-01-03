﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.General
{
    public class ResponseModel<T>
    {
        public T? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
