using PRM.Application.Model;

namespace PRM.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(Guid id);
        Task<(bool IsSuccess, string Message, CategoryDto? Data)> CreateAsync(CreateCategoryDto dto);
        Task<(bool IsSuccess, string Message, CategoryDto? Data)> UpdateAsync(Guid id, UpdateCategoryDto dto);
        Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id);
    }
}
