using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.General
{
 
    public class AuthenticateModel
    {
        public string Email { get; set; }
        public string code { get; set; }
    }
}
