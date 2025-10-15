using PRM.Application.Interfaces;
using PRM.Application.Interfaces.Repositories;
using PRM.Application.Model;
using PRM.Domain.Entities;

namespace PRM.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Status = c.Status
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Status = category.Status
            };
        }

        public async Task<CategoryWithProductsDto?> GetCategoryWithProductsAsync(Guid id)
        {
            var category = await _categoryRepository.GetCategoryWithProductsAsync(id);
            if (category == null) return null;

            return new CategoryWithProductsDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Status = category.Status,
                Products = category.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                }).ToList()
            };
        }

        public async Task<(bool IsSuccess, string Message, CategoryDto? Data)> CreateAsync(CreateCategoryDto dto)
        {
            if (await _categoryRepository.IsDuplicateNameAsync(dto.Name))
                return (false, "Duplicate category name, try another", null);

            var entity = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = dto.Name,
                Status = "active"
            };

            await _categoryRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = new CategoryDto
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Status = entity.Status
            };

            return (true, "Create category successfully", result);
        }

        public async Task<(bool IsSuccess, string Message, CategoryDto? Data)> UpdateAsync(Guid id, UpdateCategoryDto dto)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity == null)
                return (false, "Category not found", null);

            if (await _categoryRepository.IsDuplicateNameAsync(dto.Name, id))
                return (false, "Duplicate category name, try another", null);

            entity.Name = dto.Name;
            entity.Status = dto.Status;

            _categoryRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = new CategoryDto
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Status = entity.Status
            };

            return (true, "Update category successfully", result);
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity == null)
                return (false, "Category not found");

            if (entity.Status == "inactive")
                return (false, "Category already inactive");

            entity.Status = "inactive";

            _categoryRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Category marked as inactive successfully");
        }
    }
}
