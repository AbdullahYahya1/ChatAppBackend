﻿using System;
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


            var imagesFolderPath = Path.Combine("wwwroot", "images");
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            if (!string.IsNullOrWhiteSpace(imgDto.img64bit))
            {
                try
                {
                    var imageBytes = Convert.FromBase64String(imgDto.img64bit);
                    var uniqueFileName = $"{Guid.NewGuid()}.jpg";
                    var physicalPath = Path.Combine(imagesFolderPath, uniqueFileName);
                    await File.WriteAllBytesAsync(physicalPath, imageBytes);
                    var relativeImagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                    var user = await _dep.UnitOfWork.Users.GetByIdAsync(_dep.GetUserId());
                    user.ProfileImg = relativeImagePath;
                    await _dep.UnitOfWork.SaveChangesAsync();
                    return new ResponseModel<string>
                    {
                        Result = relativeImagePath,
                        IsSuccess = true,
                    };
                }
                catch (FormatException)
                {
                    return new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "image contain invalid base64 data."
                    };
                }
            }
            return new ResponseModel<string>
            {
                IsSuccess = false,
                Message = "somthing went wrong"
            };

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
            var subject = "Your Verification Code";
            var emailBody = $@"
                Hi,  

                Here is your verification code: {code}  

                Best,  
                The Support Team";
            await _emailSender.SendEmailAsync(emailDto.Email, subject, emailBody);
            return new ResponseModel<EmailCodeDto>
            {
                Message = "Verification code sent to email.",
                IsSuccess = true,
                Result = new EmailCodeDto
                {
                    Email = emailDto.Email,
                    EmailCode ="000000"
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
            if (user == null)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "User not found. Please log in again."
                };
            }

            if (user.RefreshToken != tokenRequest.RefreshToken)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "Invalid refresh token. Please request a new one."
                };
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ResponseModel<TokenResponse>
                {
                    IsSuccess = false,
                    Message = "Your session has expired. Please log in again to continue."
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
