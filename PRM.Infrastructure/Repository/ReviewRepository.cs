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
	public class ReviewRepository : IReviewRepository
	{
		private readonly PRMDbContext _context;

		public ReviewRepository(PRMDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Review>> GetByProductIdWithIncludeAsync(Guid productId)
		{
			return await _context.Reviews
				.Include(r => r.User)
				.Include(r => r.Product)
				.Where(r => r.ProductId == productId)
				.ToListAsync();
		}
	}
}
