using Core.Responses;
using ToDo.Models.Dtos.Tokens.Responses;
using ToDo.Models.Dtos.User.Request;
using ToDo.Service.Abstracts;

namespace ToDo.Service.Concretes
{
    public class AuthenticationService(IUserService _userService, IJwtService _jwtService) : IAuthenticationService
    {
        public async Task<ReturnModel<TokenResponseDto>> LoginAsync(UserLoginRequestDto loginRequestDto)
        {
            var user = await _userService.LoginAsync(loginRequestDto);
            var tokenResponseDto = await _jwtService.CreateJwtTokenAsync(user);

            return new ReturnModel<TokenResponseDto>
            {
                Data = tokenResponseDto,
                Message = "Giriş başarılı.",
                StatusCode = 200,
                Success = true
            };
        }

        public async Task<ReturnModel<TokenResponseDto>> RegisterAsync(UserRegisterRequestDto registerRequestDto)
        {
            var user = await _userService.RegisterAsync(registerRequestDto);
            var registerResponse = await _jwtService.CreateJwtTokenAsync(user);

            return new ReturnModel<TokenResponseDto>
            {
                Data = registerResponse,
                Message = "Giriş başarılı.",
                StatusCode = 200,
                Success = true
            };
        }
    }
}
