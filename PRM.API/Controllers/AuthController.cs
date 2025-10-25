using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.Application.Common;
using PRM.Application.IService;
using PRM.Application.Model.Auth;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;

		public AuthController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var user = await _userService.RegisterAsync(dto);
			return Ok(BaseResponse<UserResponseDto>.Ok(user, "Đăng ký thành công"));
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var user = await _userService.LoginAsync(dto);
			return Ok(BaseResponse<UserResponseDto>.Ok(user, "Đăng nhập thành công"));
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAllUserInformationAsync();
			return Ok(BaseResponse<IEnumerable<ProfileResponseDto>>.Ok(users));
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserById(Guid userId)
		{
			var user = await _userService.GetUserInformationAsync(userId);
			return Ok(BaseResponse<ProfileResponseDto>.Ok(user));
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.LogoutAsync();
			return Ok(BaseResponse<string>.Ok(null, "Đăng xuất thành công"));
		}

	}
}
