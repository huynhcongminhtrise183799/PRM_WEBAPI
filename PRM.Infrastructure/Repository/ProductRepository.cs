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
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly PRMDbContext _context;

		public ProductRepository(PRMDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
		{
			return await _context.Set<Product>()
				.Include(p => p.Category)
				.Include(p => p.Supplier)
				.Include(p => p.ProductColors)
					.ThenInclude(pc => pc.ProductImages)
				.ToListAsync();
		}

		public async Task<Product?> GetByIdWithDetailsAsync(Guid id)
		{
			return await _context.Set<Product>()
				.Include(p => p.Category)
				.Include(p => p.Supplier)
				.Include(p => p.ProductColors)
					.ThenInclude(pc => pc.ProductImages)
				.FirstOrDefaultAsync(p => p.ProductId == id);
		}
	}
}
