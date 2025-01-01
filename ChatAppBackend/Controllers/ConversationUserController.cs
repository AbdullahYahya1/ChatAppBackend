using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationUserController : ControllerBase
    {
        [HttpPost("AddChatUser")]
        public async Task<IActionResult> AddChat(EmailDto emailDto)
        {
            return Ok(); 
        }
    }
}
