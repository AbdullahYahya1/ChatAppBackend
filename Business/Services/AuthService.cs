using Business.Entities;
using Business.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        string IAuthService.GenerateJwtToken(User user)
        {
            throw new NotImplementedException();
        }

        string IAuthService.GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        ClaimsPrincipal IAuthService.GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
