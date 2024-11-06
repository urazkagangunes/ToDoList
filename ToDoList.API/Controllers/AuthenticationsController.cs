using Microsoft.AspNetCore.Mvc;
using ToDo.Models.Dtos.User.Request;
using ToDo.Service.Abstracts;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController(IAuthenticationService _authenticationService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequestDto loginRequestDto)
        {
            var result = await _authenticationService.LoginAsync(loginRequestDto);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAync([FromBody] UserRegisterRequestDto registerRequestDto)
        {
            var result = await _authenticationService.RegisterAsync(registerRequestDto);
            return Ok(result);
        }
    }
}
