using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.API.DTOs.Request;
using PRM.Application.IService;
using PRM.Application.Model;

namespace PRM.API.Controllers
{
	[Route("api/")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IUserDeviceTokenService _userDeviceTokenService;
		public UserController(IUserService userService, IUserDeviceTokenService userDeviceTokenService)
		{
			_userService = userService;
			_userDeviceTokenService = userDeviceTokenService;
		}
		[HttpGet("admin")]
		public async Task<IActionResult> GetAdmin()
		{
			var result = await _userService.GetAdmin();
			return Ok(result);
		}
		[HttpPost("user/device-token")]
		public async Task<IActionResult> AddUserDeviceToken([FromBody] UserDeviceTokenRequest userDeviceToken)
		{
			var model = new UserDeviceTokenModel
			{
				UserId = userDeviceToken.UserId,
				FCMToken = userDeviceToken.FCMToken,
			};
			var result = await _userDeviceTokenService.AddToken(model);
			return Ok(result);
		}
	}
}
