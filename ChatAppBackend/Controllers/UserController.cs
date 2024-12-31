using Business.Entities;
using Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServicesDependency<User> _servicesDependency;
        public UserController(IServicesDependency<User> servicesDependency) {
            _servicesDependency = servicesDependency;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            Console.WriteLine("GetUserId: "+ _servicesDependency.GetUserId());
            Console.WriteLine("GetUserId: "+ _servicesDependency.GetUserId());
            Console.WriteLine("GetUserId: " + _servicesDependency.GetUserId());
            Console.WriteLine("GetUserId: " + _servicesDependency.GetUserId());
            return Ok("here we are"); 
        }
    }
}
