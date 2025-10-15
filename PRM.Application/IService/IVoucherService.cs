using PRM.Application.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRM.Application.Interfaces
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherDto>> GetAllAsync();
        Task<VoucherDto?> GetByIdAsync(Guid id);
        Task<(bool IsSuccess, string Message, VoucherDto? Data)> CreateAsync(CreateVoucherDto dto);
        Task<(bool IsSuccess, string Message, VoucherDto? Data)> UpdateAsync(Guid id, UpdateVoucherDto dto);
        Task<(bool IsSuccess, string Message)> DeleteAsync(Guid id);
        Task<VoucherDto?> GetByCodeAsync(string code);
    }
}
