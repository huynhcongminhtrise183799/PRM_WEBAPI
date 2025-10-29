using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.API.DTOs.Response;
using PRM.Application.Common;
using PRM.Application.IService;
using PRM.Application.Model.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		[HttpPost("register/admin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto)
		{
			var user = await _userService.RegisterAdminAsync(dto);
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

		[HttpPut("{userId}/update-profile")]
		public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileRequestDto updateDto)
		{
			var updatedUser = await _userService.UpdateProfileAsync(userId, updateDto);
			return Ok(BaseResponse<UserResponseDto>.Ok(updatedUser, "Cập nhật thông tin thành công"));
		}

		[HttpGet("total-user")]
		public async Task<IActionResult> GetTotalUsers()
		{
			var total = await _userService.GetTotalUsersAsync();
			var result = new DTOs.Response.TotalUserResponseDto
			{
				TotalUser = total
			};
			return Ok(BaseResponse<DTOs.Response.TotalUserResponseDto>.Ok(result));
		}
	}
}
