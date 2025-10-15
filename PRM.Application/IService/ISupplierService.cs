using PRM.Application.Model;

namespace PRM.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(Guid id);
        Task<SupplierWithProductsDto?> GetSupplierWithProductsAsync(Guid id);
        Task<(bool IsSuccess, string Message, SupplierDto? Data)> CreateAsync(CreateSupplierDto dto);
        Task<(bool IsSuccess, string Message, SupplierDto? Data)> UpdateAsync(Guid id, UpdateSupplierDto dto);
        Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id);
    }
}
