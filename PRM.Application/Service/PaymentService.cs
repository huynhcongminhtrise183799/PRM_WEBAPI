
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PRM.Application.Helper;
using PRM.Application.IService;
using PRM.Application.Model.Payment;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace PRM.Application.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _config;
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IOrderService _orderService;
		private readonly IPaymentRepository _paymentRepository;
		public PaymentService(IConfiguration config, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IOrderService orderService, IHttpContextAccessor httpContextAccessor, IPaymentRepository paymentRepository)
		{
			_config = config;
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
			_orderService = orderService;
			_httpContextAccessor = httpContextAccessor;
			_paymentRepository = paymentRepository;
		}

		public async Task<PaymentResponseDto> CreatePaymentUrlAsync(Guid userId)
		{
			var order = await _orderService.CreateOrderFromCartAsync(userId);
			await _unitOfWork.SaveChangesAsync();

			var vnPay = new VnPayLibrary();
			var httpContext = _httpContextAccessor.HttpContext;

			vnPay.AddRequestData("vnp_Version", _config["Vnpay:Version"]);
			vnPay.AddRequestData("vnp_Command", _config["Vnpay:Command"]);
			vnPay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]);
			vnPay.AddRequestData("vnp_Amount", ((int)(order.TotalAmount * 100)).ToString());
			vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
			vnPay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]);
			vnPay.AddRequestData("vnp_IpAddr", vnPay.GetIpAddress(httpContext));
			vnPay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]);
			vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.OrderId}");
			vnPay.AddRequestData("vnp_OrderType", "other");
			vnPay.AddRequestData("vnp_ReturnUrl", _config["Vnpay:ReturnUrl"]);
			vnPay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

			string paymentUrl = vnPay.CreateRequestUrl(
				_config["Vnpay:BaseUrl"],
				_config["Vnpay:HashSecret"]
			);

			return new PaymentResponseDto
			{
				OrderId = order.OrderId,
				PaymentUrl = paymentUrl
			};
		}

		public async Task<bool> PaymentCallbackAsync(IQueryCollection vnp_Params)
		{
			string vnp_HashSecret = _config["Vnpay:HashSecret"];
			var vnPay = new VnPayLibrary();

			foreach (var key in vnp_Params.Keys)
			{
				vnPay.AddResponseData(key, vnp_Params[key]);
			}

			string vnp_SecureHash = vnp_Params["vnp_SecureHash"];
			bool validSignature = vnPay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

			if (!validSignature)
				return false;

			if (vnp_Params["vnp_ResponseCode"] == "00")
			{
				var orderId = Guid.Parse(vnp_Params["vnp_TxnRef"]);
				var order = await _orderRepository.GetByIdAsync(orderId);
				order.PaymentStatus = "Paid";
				order.Status = "Completed";
				await _unitOfWork.SaveChangesAsync();
				return true;
			}

			return false;
		}

		public async Task<(bool IsSuccess, string Message)> UpdatePaymentStatusAsync(Guid orderId, string responseCode)
		{

			var order = await _orderRepository.GetByIdAsync(orderId);
			if (order == null)
				return (false, "Order not found.");


			var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
			if (payment == null)
				return (false, "Payment record not found.");

			if (payment == null)
				return (false, "Payment record not found.");


			if (responseCode == "00")
			{

				order.PaymentStatus = "Paid";
				order.Status = "Processing";
				payment.Status = "Paid";
				payment.PaymentDate = DateOnly.FromDateTime(DateTime.Now);

				await _unitOfWork.SaveChangesAsync();
				return (true, "Payment successful.");
			}
			else
			{
				order.PaymentStatus = "Unpaid";
				order.Status = "Cancelled";
				payment.Status = "Unpaid";
				payment.PaymentDate = DateOnly.FromDateTime(DateTime.Now);

				await _unitOfWork.SaveChangesAsync();
				return (false, $"Payment failed. Code: {responseCode}");
			}
		}

		public async Task<long> GetTotalPaidAmountByDateAsync(DateTime date)
		{
			var paymentRepo = _unitOfWork.Repository<Payments>();
			var targetDate = DateOnly.FromDateTime(date);

			var payments = await paymentRepo.GetAllAsync();

			var filteredPayments = payments
				.Where(p => p.Status == "Paid" && p.PaymentDate == targetDate)
				.ToList();

			if (!filteredPayments.Any())
				return 0;

			return (long)filteredPayments.Sum(p => p.Amount);
		}
	}
}
