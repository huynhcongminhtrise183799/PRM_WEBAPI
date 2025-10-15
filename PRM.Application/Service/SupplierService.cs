using PRM.Application.Interfaces;
using PRM.Application.Interfaces.Repositories;
using PRM.Application.Model;
using PRM.Domain.Entities;

namespace PRM.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return suppliers.Select(s => new SupplierDto
            {
                SupplierId = s.SupplierId,
                Name = s.Name
            });
        }

        public async Task<SupplierDto?> GetByIdAsync(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return null;

            return new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name
            };
        }

        public async Task<SupplierWithProductsDto?> GetSupplierWithProductsAsync(Guid id)
        {
            var supplier = await _supplierRepository.GetSupplierWithProductsAsync(id);
            if (supplier == null) return null;

            return new SupplierWithProductsDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                Products = supplier.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                }).ToList()
            };
        }

        public async Task<(bool IsSuccess, string Message, SupplierDto? Data)> CreateAsync(CreateSupplierDto dto)
        {
            if (await _supplierRepository.IsDuplicateNameAsync(dto.Name))
                return (false, "Duplicate supplier name, try another", null);

            var entity = new Suppliers
            {
                SupplierId = Guid.NewGuid(),
                Name = dto.Name
            };

            await _supplierRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = new SupplierDto
            {
                SupplierId = entity.SupplierId,
                Name = entity.Name
            };

            return (true, "Create supplier successfully", result);
        }

        public async Task<(bool IsSuccess, string Message, SupplierDto? Data)> UpdateAsync(Guid id, UpdateSupplierDto dto)
        {
            var entity = await _supplierRepository.GetByIdAsync(id);
            if (entity == null)
                return (false, "Supplier not found", null);

            if (await _supplierRepository.IsDuplicateNameAsync(dto.Name, id))
                return (false, "Duplicate supplier name, try another", null);

            entity.Name = dto.Name;

            _supplierRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = new SupplierDto
            {
                SupplierId = entity.SupplierId,
                Name = entity.Name
            };

            return (true, "Update supplier successfully", result);
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id)
        {
            var entity = await _supplierRepository.GetByIdAsync(id);
            if (entity == null)
                return (false, "Supplier not found");

            _supplierRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Delete supplier successfully");
        }
    }
}
