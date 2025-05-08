using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IContractRepository
    {
        Task<IEnumerable<GetAllContractsDto>> GetAllContractsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<GetAllContractsDto>> GetActiveContractsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<GetAllContractsDto>> GetTerminatedContractsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<GetAllContractsDto>> GetPendingApprovalContractsAsync(int pageNumber, int pageSize);
        Task<GetContractByIdDto> GetContractByIdAsync(int id);
        Task<Contract> AddContractAsync(Contract cp);
        Task<bool> UpdateContractAsync(int id, Contract cp);
        Task<bool> DeleteContractAsync(int id);
    }
}
