using Business.Entities;
using DataAccess.Dtos.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAuthService
    {
        internal string GenerateJwtToken(User user);
        internal string GenerateRefreshToken();
        internal ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
