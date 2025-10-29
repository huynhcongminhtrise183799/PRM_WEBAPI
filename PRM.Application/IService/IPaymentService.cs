using Microsoft.AspNetCore.Http;
using PRM.Application.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
	public interface IPaymentService
	{
		Task<PaymentResponseDto> CreatePaymentUrlAsync(Guid userId);
		Task<bool> PaymentCallbackAsync(IQueryCollection vnp_Params);
		Task<(bool IsSuccess, string Message)> UpdatePaymentStatusAsync(Guid orderId, string responseCode);
		Task<long> GetTotalPaidAmountByDateAsync(DateTime date);
	}
}
