using Business.Entities;
using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        public Task<ResponseModel<AuthenticationResponse>> Authenticate(string email)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseModel<bool>> CreateUser(PostUserDto userDto)
        {
            return new ResponseModel<bool>()
            {
                Message = "good",
                IsSuccess = true,
            };
        }
        public Task<ResponseModel<AuthenticationResponse>> CustomerAuthenticate(string phone)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<TokenResponse>> RefreshToken(TokenRequest tokenRequest)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<bool>> UpdateUser(PutUserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
