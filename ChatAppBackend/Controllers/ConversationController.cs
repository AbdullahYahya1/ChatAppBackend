using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationServices _conversationServices;
        public ConversationController(IConversationServices conversationServices)
        {
            _conversationServices = conversationServices;
        }
        [HttpPost("CreateChatUser")]
        public async Task<IActionResult> AddChat(EmailDto emailDto)
        {
            var res = await _conversationServices.AddChat(emailDto);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Conversations([FromQuery] Pagination pagination,[FromQuery] string? email)
        {
            var res = await _conversationServices.GetCurrentConversations(pagination, email);
            return Ok(res); 
        }
        
        
    }
}
