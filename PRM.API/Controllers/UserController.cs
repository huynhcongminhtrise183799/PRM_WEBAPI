using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;

namespace PRM.API.Controllers
{
	[Route("api/")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		[HttpGet("admin")]
		public async Task<IActionResult> GetAdmin()
		{
			var result = await _userService.GetAdmin();
			return Ok(result);
		}
	}
}
