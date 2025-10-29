using Microsoft.EntityFrameworkCore;
using PRM.Domain.Entities;
using PRM.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Infrastructure.Repository
{
	public class PaymentRepository : GenericRepository<Payments>, IPaymentRepository
	{

		private readonly PRMDbContext _context;

		public PaymentRepository(PRMDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Payments?> GetByOrderIdAsync(Guid orderId)
		{
			return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
		}
	}
}
