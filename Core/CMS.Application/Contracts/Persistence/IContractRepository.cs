using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllContractsAsync(int pageNumber, int pageSize);
        Task<Contract> GetContractByIdAsync(int id);
        Task<Contract> AddContractAsync(Contract cp);
        Task<bool> UpdateContractAsync(int id, Contract cp);
        Task<bool> DeleteContractAsync(int id);
    }
}
