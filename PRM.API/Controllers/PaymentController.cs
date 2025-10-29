using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;

namespace PRM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class PaymentController : Controller
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpGet("create")]
		public async Task<IActionResult> CreatePaymentUrl(Guid userId)
		{
			try
			{
				var paymentUrl = await _paymentService.CreatePaymentUrlAsync(userId);
				return Ok(new { paymentUrl });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("callback")]
		public async Task<IActionResult> PaymentCallback()
		{
			try
			{
				var result = await _paymentService.PaymentCallbackAsync(Request.Query);
				if (result)
				{
					return Ok(new { message = "Thanh toán thành công" });
				}
				else
				{
					return BadRequest(new { message = "Thanh toán thất bại hoặc bị hủy" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("update")]
		public async Task<IActionResult> UpdatePaymentStatus([FromForm] Guid orderId, [FromForm] string responseCode)
		{
			var (isSuccess, message) = await _paymentService.UpdatePaymentStatusAsync(orderId, responseCode);

			if (isSuccess)
				return Ok(new { success = true, message });
			return BadRequest(new { success = false, message });
		}
	}
}
