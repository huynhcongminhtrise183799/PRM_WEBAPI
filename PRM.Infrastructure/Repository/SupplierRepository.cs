using Microsoft.EntityFrameworkCore;
using PRM.Application.Interfaces.Repositories;
using PRM.Domain.Entities;

namespace PRM.Infrastructure.Repositories
{
    public class SupplierRepository : GenericRepository<Suppliers>, ISupplierRepository
    {
        private readonly PRMDbContext _context;

        public SupplierRepository(PRMDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Suppliers?> GetSupplierWithProductsAsync(Guid supplierId)
        {
            return await _context.Suppliers
                .Include(s => s.Products)
                    .ThenInclude(p => p.ProductColors)
                    .ThenInclude(pc => pc.ProductImages)
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);
        }

        public async Task<bool> IsDuplicateNameAsync(string name, Guid? excludeId = null)
        {
            return await _context.Suppliers
                .AnyAsync(s => s.Name.ToLower() == name.ToLower() && (excludeId == null || s.SupplierId != excludeId));
        }
    }
}
