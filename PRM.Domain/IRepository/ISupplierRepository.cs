using PRM.Domain.Entities;

namespace PRM.Application.Interfaces.Repositories
{
    public interface ISupplierRepository : IGenericRepository<Suppliers>
    {
        Task<Suppliers?> GetSupplierWithProductsAsync(Guid supplierId);
        Task<bool> IsDuplicateNameAsync(string name, Guid? excludeId = null);
    }
}
