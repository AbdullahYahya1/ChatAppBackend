using Business.FileService;
using Business.IServices;
using DataAccess.Dtos.MessageDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{ConversationID}")]
        public async Task<IActionResult> GetMessages(int ConversationID,[FromQuery] int? lastMessageId) {
            var res = await _messageService.GetMessages(ConversationID, lastMessageId);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromForm]  PostMessageDto postMessageDto)
        {
            var res = await _messageService.PostMessage(postMessageDto);
            return Ok(res);
        }
    }
}
