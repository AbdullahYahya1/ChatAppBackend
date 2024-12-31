using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.IServices;
using DataAccess.Dtos.UserDtos;
using DataAccess.Dtos.General;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("requestEmailCode")]
    public async Task<IActionResult> RequestEmailCode([FromBody] EmailDto emailDto)
    {
        var res = await _userService.requestEmailCode(emailDto);
        return Ok(res);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateModel model)
    {
        var tokens = await _userService.Authenticate(model.Email, model.code);
        return Ok(tokens);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        var response = await _userService.RefreshToken(tokenRequest);
        return Ok(response);
    }
}
