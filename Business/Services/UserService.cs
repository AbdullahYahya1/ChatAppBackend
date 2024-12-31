using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Entities;
using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailSender _emailSender;
        private readonly IServicesDependency<User> _dep;
        private readonly IAuthService _authService;
        private static readonly Dictionary<string, string> verificationCodes = new Dictionary<string, string>();

        public UserService(
            IEmailSender emailSender,
            IServicesDependency<User> dep,
            IAuthService authService)
        {
            _dep = dep;
            _emailSender = emailSender;
            _authService = authService;
        }

        public async Task<ResponseModel<EmailCodeDto>> requestEmailCode(EmailDto emailDto)
        {
            var code = new Random().Next(100000, 999999).ToString();
            verificationCodes[emailDto.Email] = code;
            var subject = "Hi, Your Verification Code";
            var message = $"verification code is: {code}";
            await _emailSender.SendEmailAsync(emailDto.Email, subject, message);
            return new ResponseModel<EmailCodeDto>
            {
                Message = "Verification code sent to email.",
                IsSuccess = true,
                Result = new EmailCodeDto
                {
                    Email = emailDto.Email,
                    EmailCode = code
                }
            };
        }

        public async Task<ResponseModel<AuthenticationResponse>> Authenticate(string email, string code)
        {
            if (!verificationCodes.ContainsKey(email) || verificationCodes[email] != code)
            {
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid or expired code."
                };
            }
            verificationCodes.Remove(email);
            var user = await _dep.UnitOfWork.Users.GetUserByEmail(email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Username = email,
                    OnlineStatus=false,
                    LastTimeConnected = DateTime.Now,
                    CreateDate= DateTime.UtcNow
                };
                await _dep.UnitOfWork.Users.AddAsync(user);
                await _dep.UnitOfWork.SaveChangesAsync();
            }
            var accessToken = _authService.GenerateJwtToken(user);
            var refreshToken = _authService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _dep.UnitOfWork.Users.UpdateAsync(user);
            await _dep.UnitOfWork.SaveChangesAsync();

            return new ResponseModel<AuthenticationResponse>
            {
                IsSuccess = true,
                Message = "User authenticated successfully.",
                Result = new AuthenticationResponse
                {
                    Tokens = new TokenResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    },
                    User = new GetUserDto
                    {
                        Email = user.Email
                    }
                }
            };
        }

        public async Task<ResponseModel<TokenResponse>> RefreshToken(TokenRequest tokenRequest)
        {
            var principal = _authService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            if (principal == null)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid token."
                };
            }
            var email = principal.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid token claims."
                };
            }
            var user = await _dep.UnitOfWork.Users.GetUserByEmail(email);
            if (user == null ||
                user.RefreshToken != tokenRequest.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "Refresh token is invalid or has expired."
                };
            }
            var newAccessToken = _authService.GenerateJwtToken(user);
            var newRefreshToken = _authService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _dep.UnitOfWork.Users.UpdateAsync(user);
            await _dep.UnitOfWork.SaveChangesAsync();
            return new ResponseModel<TokenResponse>
            {
                IsSuccess = true,
                Message = "Token refreshed successfully.",
                Result = new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }
    }
}
