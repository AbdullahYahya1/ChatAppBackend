using Business.IServices;
using DataAccess.Dtos.General;
using DataAccess.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;

//AB
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    //IUserService userService, ILogger<AuthenticationController> logger
    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] PostUserDto userDto)
    {
        var res = await _userService.CreateUser(userDto);
        return Ok(res);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateModel model)
    {
        var tokens = await _userService.Authenticate(model.Email);
        return Ok(tokens);
    }


    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        var response = await _userService.RefreshToken(tokenRequest);
        return Ok(response);
    }
}
