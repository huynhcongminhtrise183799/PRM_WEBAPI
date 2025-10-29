using PRM.Application.IService;
using PRM.Application.Model.Order;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.Service
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IOrderRepository _orderRepository;
		private readonly ICartRepository _cartRepository;
		private readonly ICartItemRepository _cartItemRepository;
		public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ICartRepository cartRepository, ICartItemRepository itemRepository)
		{
			_unitOfWork = unitOfWork;
			_orderRepository = orderRepository;
			_cartRepository = cartRepository;
			_cartItemRepository = itemRepository;
		}

		public async Task<Order> CreateOrderFromCartAsync(Guid userId)
		{
			var cart = await _cartRepository.GetCartWithItemsAsync(userId);

			if (cart == null)
				throw new Exception("Cart not found.");

			if (cart.CartItems == null || !cart.CartItems.Any())
				throw new Exception("Cart is empty.");

			var order = new Order
			{
				OrderId = Guid.NewGuid(),
				UserId = userId,
				Status = "Pending",
				PaymentStatus = "Unpaid",
				TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price),
				OrderItems = cart.CartItems.Select(ci => new OrderItem
				{
					OrderItemId = Guid.NewGuid(),
					ProductColorId = ci.ProductColorId,
					Quantity = ci.Quantity,
					UnitPrice = ci.Price,
				}).ToList()
			};
			var payment = new Payments
			{
				PaymentId = Guid.NewGuid(),
				OrderId = order.OrderId,
				Amount = order.TotalAmount,
				Method = "VNPay",
				Status = "Pending",
				PaymentDate = DateOnly.FromDateTime(DateTime.Now)
			};

			await _unitOfWork.Repository<Payments>().AddAsync(payment);
			await _orderRepository.AddAsync(order);

			
			_cartItemRepository.DeleteRange(cart.CartItems);
			await _unitOfWork.SaveChangesAsync();

			return order;
		}

		public async Task<List<OrderHistoryDto>> GetOrderHistoryByUserAsync(Guid userId)
		{
			var orders = await _orderRepository.GetOrdersByUserAsync(userId);

			var orderDtos = orders.Select(o => new OrderHistoryDto
			{
				OrderId = o.OrderId,
				TotalAmount = o.TotalAmount,
				Status = o.Status,
				PaymentStatus = o.PaymentStatus,
				PaymentMethod = o.Payments?.Method ?? "N/A",
				PaymentDate = o.Payments?.PaymentDate
			}).ToList();

			return orderDtos;
		}
	}
}
