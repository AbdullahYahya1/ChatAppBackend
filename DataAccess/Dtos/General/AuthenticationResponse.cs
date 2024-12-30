using DataAccess.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dtos.General
{
    public class AuthenticationResponse
    {
        public TokenResponse Tokens { get; set; }
        public GetUserDto User { get; set; }
    }
}
