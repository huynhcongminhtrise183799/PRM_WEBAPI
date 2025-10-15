using PRM.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace PRM.Application.Interfaces.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<Voucher?> GetByCodeAsync(string code);
        Task<bool> IsDuplicateCodeAsync(string code, Guid? excludeId = null);
    }
}
