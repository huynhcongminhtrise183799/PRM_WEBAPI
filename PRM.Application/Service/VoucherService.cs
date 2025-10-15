using PRM.Application.Interfaces;
using PRM.Application.Interfaces.Repositories;
using PRM.Application.Model;
using PRM.Domain.Entities;

namespace PRM.Application.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService(IUnitOfWork unitOfWork, IVoucherRepository voucherRepository)
        {
            _unitOfWork = unitOfWork;
            _voucherRepository = voucherRepository;
        }

        private void UpdateVoucherStatusIfExpired(Voucher voucher)
        {
            if (voucher.ExpiryDate < DateTime.UtcNow && voucher.Status.ToLower() != "inactive")
            {
                voucher.Status = "inactive";
            }
        }

        public async Task<IEnumerable<VoucherDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Voucher>();
            var vouchers = (await repo.GetAllAsync()).ToList();

            foreach (var v in vouchers)
            {
                UpdateVoucherStatusIfExpired(v);
            }

            await _unitOfWork.SaveChangesAsync();

            return vouchers.Select(v => new VoucherDto
            {
                VoucherId = v.VoucherId,
                Code = v.Code,
                Description = v.Description,
                DiscountValue = v.DiscountValue,
                StartDate = v.StartDate,
                ExpiryDate = v.ExpiryDate,
                UsageLimit = v.UsageLimit,
                Status = v.Status
            });
        }

        public async Task<VoucherDto?> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<Voucher>();
            var voucher = await repo.GetByIdAsync(id);
            if (voucher == null) return null;

            UpdateVoucherStatusIfExpired(voucher);
            await _unitOfWork.SaveChangesAsync();

            return new VoucherDto
            {
                VoucherId = voucher.VoucherId,
                Code = voucher.Code,
                Description = voucher.Description,
                DiscountValue = voucher.DiscountValue,
                StartDate = voucher.StartDate,
                ExpiryDate = voucher.ExpiryDate,
                UsageLimit = voucher.UsageLimit,
                Status = voucher.Status
            };
        }

        public async Task<VoucherDto?> GetByCodeAsync(string code)
        {
            var voucher = await _voucherRepository.GetByCodeAsync(code);
            if (voucher == null) return null;

            UpdateVoucherStatusIfExpired(voucher);
            await _unitOfWork.SaveChangesAsync();

            return new VoucherDto
            {
                VoucherId = voucher.VoucherId,
                Code = voucher.Code,
                Description = voucher.Description,
                DiscountValue = voucher.DiscountValue,
                StartDate = voucher.StartDate,
                ExpiryDate = voucher.ExpiryDate,
                UsageLimit = voucher.UsageLimit,
                Status = voucher.Status
            };
        }

        public async Task<(bool IsSuccess, string Message, VoucherDto? Data)> CreateAsync(CreateVoucherDto dto)
        {
            if (await _voucherRepository.IsDuplicateCodeAsync(dto.Code))
                return (false, "Duplicate voucher code", null);

            var entity = new Voucher
            {
                VoucherId = Guid.NewGuid(),
                Code = dto.Code,
                Description = dto.Description,
                DiscountValue = dto.DiscountValue,
                StartDate = dto.StartDate,
                ExpiryDate = dto.ExpiryDate,
                UsageLimit = dto.UsageLimit,
                Status = "active"
            };

            await _unitOfWork.Repository<Voucher>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Voucher created successfully", new VoucherDto
            {
                VoucherId = entity.VoucherId,
                Code = entity.Code,
                Description = entity.Description,
                DiscountValue = entity.DiscountValue,
                StartDate = entity.StartDate,
                ExpiryDate = entity.ExpiryDate,
                UsageLimit = entity.UsageLimit,
                Status = entity.Status
            });
        }

        public async Task<(bool IsSuccess, string Message, VoucherDto? Data)> UpdateAsync(Guid id, UpdateVoucherDto dto)
        {
            var repo = _unitOfWork.Repository<Voucher>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return (false, "Voucher not found", null);

            if (await _voucherRepository.IsDuplicateCodeAsync(dto.Code, id))
                return (false, "Duplicate voucher code", null);

            entity.Code = dto.Code;
            entity.Description = dto.Description;
            entity.DiscountValue = dto.DiscountValue;
            entity.StartDate = dto.StartDate;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.UsageLimit = dto.UsageLimit;
            entity.Status = dto.Status;

            UpdateVoucherStatusIfExpired(entity);
            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Voucher updated successfully", new VoucherDto
            {
                VoucherId = entity.VoucherId,
                Code = entity.Code,
                Description = entity.Description,
                DiscountValue = entity.DiscountValue,
                StartDate = entity.StartDate,
                ExpiryDate = entity.ExpiryDate,
                UsageLimit = entity.UsageLimit,
                Status = entity.Status
            });
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<Voucher>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return (false, "Voucher not found");

            if (entity.Status.ToLower() == "inactive")
                return (false, "Voucher is already inactive");

            entity.Status = "inactive";
            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Voucher deactivated successfully");
        }
    }
}
