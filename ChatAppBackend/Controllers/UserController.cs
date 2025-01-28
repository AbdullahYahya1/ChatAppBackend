using Business.Entities;
using Business.IServices;
using DataAccess.Dtos.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;

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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddImageToProfile(imgDto imgDto)
        {
            
            return Ok(); 
        }
    }
}
