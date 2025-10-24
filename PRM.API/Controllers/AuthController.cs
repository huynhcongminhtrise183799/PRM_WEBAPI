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
			var result = await _userService.RegisterAsync(dto);
			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var result = await _userService.LoginAsync(dto);
			return Ok(result);
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllUsers()
		{
			var result = await _userService.GetAllUserInformationAsync();
			return Ok(result);
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserById(Guid userId)
		{
			var result = await _userService.GetUserInformationAsync(userId);
			return Ok(result);
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.LogoutAsync();
			return Ok(new { message = "Đăng xuất thành công." });
		}

	}
}
