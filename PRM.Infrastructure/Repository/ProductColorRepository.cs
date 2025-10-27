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
	public class ProductColorRepository :GenericRepository<ProductColors>,  IProductColorRepository
	{
		private readonly PRMDbContext _context;

		public ProductColorRepository(PRMDbContext context) : base(context) {

			_context = context;
		}

		public async Task<ProductColors?> GetByIdAsync(Guid id)
		{
			return await _context.ProductColors
				.Include(pc => pc.Product)
				.FirstOrDefaultAsync(pc => pc.ProductColorsId == id);
		}
	}
}

