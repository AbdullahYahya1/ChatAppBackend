using Business.IServices;
using DataAccess.Dtos.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService){
            _messageService = messageService;
        }


        [HttpGet("{ConversationID}")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int ConversationID) {

            return Ok("here we are");
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostMessage(PostMessageDto postMessageDto)
        {
            var res = _messageService.PostMessage(postMessageDto);
            return Ok(res);
        }
    }
}
