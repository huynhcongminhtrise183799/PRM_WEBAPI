using PRM.Domain.Entities;

namespace PRM.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryWithProductsAsync(Guid categoryId);
        Task<bool> IsDuplicateNameAsync(string name, Guid? excludeId = null);
    }
}
