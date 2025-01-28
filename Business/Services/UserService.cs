using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Entities;
using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailSender _emailSender;
        private readonly IServicesDependency<User> _dep;
        private readonly IAuthService _authService;

        // We'll store code + the DateTime it was created in a tuple: (Code, CreatedAt)
        private static readonly Dictionary<string, (string Code, DateTime CreatedAt)> verificationCodes
            = new Dictionary<string, (string Code, DateTime CreatedAt)>();

        // The time window for which a verification code is valid (2 minutes).
        private static readonly TimeSpan CodeValidity = TimeSpan.FromMinutes(2);

        public UserService(
            IEmailSender emailSender,
            IServicesDependency<User> dep,
            IAuthService authService)
        {
            _dep = dep;
            _emailSender = emailSender;
            _authService = authService;
        }

        public async Task<ResponseModel<string>> UpdateImage(imgDto imgDto){
            throw new NotImplementedException(); 
        }

        public async Task<ResponseModel<EmailCodeDto>> requestEmailCode(EmailDto emailDto)
        {
            if (verificationCodes.TryGetValue(emailDto.Email, out var existingRecord))
            {
                var timeSinceCreation = DateTime.UtcNow - existingRecord.CreatedAt;
                if (timeSinceCreation < CodeValidity)
                {
                    return new ResponseModel<EmailCodeDto>
                    {
                        IsSuccess = false,
                        Message = "A verification code was already sent. Please wait 2 minutes before requesting a new one.",
                        Result = null
                    };
                }
                else
                {
                    verificationCodes.Remove(emailDto.Email);
                }
            }
            var code = new Random().Next(100000, 999999).ToString();
            verificationCodes[emailDto.Email] = (code, DateTime.UtcNow);
            var subject = "Hi, Your Verification Code";
            var emailBody = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                            color: #333;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #fff;
                            padding: 20px;
                            border: 1px solid #ddd;
                            border-radius: 5px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #444;
                            font-size: 24px;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.5;
                        }}
                        .code {{
                            font-weight: bold;
                            color: #e74c3c;
                            font-size: 18px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <h1>Hi, Your Verification Code</h1>
                        <p>Thank you for using our service. Please use the verification code below to proceed:</p>
                        <p class='code'>{code}</p>
                        <p>If you didn’t request this email, please ignore it.</p>
                        <p>Best regards, <br /> The Support Team</p>
                    </div>
                </body>
            </html>";
            //await _emailSender.SendEmailAsync(emailDto.Email, subject, emailBody);
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
            if (!verificationCodes.TryGetValue(email, out var storedCodeInfo))
            {
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid or expired code."
                };
            }
            if (storedCodeInfo.Code != code)
            {
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid or expired code."
                };
            }
            var timeSinceCreation = DateTime.UtcNow - storedCodeInfo.CreatedAt;
            if (timeSinceCreation > CodeValidity)
            {
                verificationCodes.Remove(email);
                return new ResponseModel<AuthenticationResponse>
                {
                    IsSuccess = false,
                    Message = "Your verification code has expired. Please request a new one."
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
                    OnlineStatus = false,
                    LastTimeConnected = DateTime.Now,
                    CreateDate = DateTime.UtcNow
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
                        UserID= user.UserID,
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
            Console.WriteLine();
            Console.WriteLine(user.Email + " "+user.RefreshToken +" " + tokenRequest.RefreshToken);
            Console.WriteLine();

            if (user == null ||
                user.RefreshToken != tokenRequest.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = user.RefreshTokenExpiryTime <= DateTime.UtcNow? "Refresh token Expired" : "Refresh token is invalid" 
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
