using Business.Entities;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IUserService
    {
        Task<ResponseModel<TokenResponse>> RefreshToken(TokenRequest tokenRequest);
        Task<ResponseModel<AuthenticationResponse>> Authenticate(string email, string code);
        Task<ResponseModel<EmailCodeDto>> requestEmailCode(EmailDto emailDto);
    }
}
