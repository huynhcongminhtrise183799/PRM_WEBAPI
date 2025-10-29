using Microsoft.AspNetCore.Mvc;
using PRM.Application.IService;

namespace PRM.API.Controllers
{
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}


	}
}
