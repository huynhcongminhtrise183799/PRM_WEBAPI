using Microsoft.EntityFrameworkCore;
using PRM.Application.Interfaces.Repositories;
using PRM.Domain.Entities;

namespace PRM.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly PRMDbContext _context;

        public CategoryRepository(PRMDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryWithProductsAsync(Guid categoryId)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> IsDuplicateNameAsync(string name, Guid? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && (excludeId == null || c.CategoryId != excludeId));
        }
    }
}
