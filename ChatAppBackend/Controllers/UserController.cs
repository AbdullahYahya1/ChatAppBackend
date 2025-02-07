using Business.Entities;
using Business.Hubs;
using Business.IServices;
using DataAccess.Dtos.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MimeKit.Cryptography;
using MimeKit.Tnef;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService = userService;
        }
        [HttpPost("ChangeProfilePage")]
        [Authorize]
        public async Task<IActionResult> AddImageToProfile(imgDto imgDto)
        {
           var res = await _userService.UpdateImage(imgDto);
            return Ok(res); 
        }

        [HttpGet("UserActive/{email}")]
        public IActionResult IsUserOnline(string email)
        {
            bool isOnline = UserHub.IsUserOnline(email);
            var RES = new ResponseModel<bool>
            {
                Result = isOnline,
                IsSuccess = true,
            };
            return Ok(RES);
        }
    }
}
