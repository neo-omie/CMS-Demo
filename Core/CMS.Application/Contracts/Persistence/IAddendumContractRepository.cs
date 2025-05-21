using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.AddendumContractDto;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Contracts.Persistence
{
    public interface IAddendumContractRepository
    {
        Task<(IEnumerable<AddendumContract>Data, int TotalCount)> GetAllAddendumContractsAsync(int pageNumber, int pageSize, DateTime? searchTerm);

        Task<(IEnumerable<AddendumContract> Data, int TotalCount)> GetAllAddendumByContractIdAsync(int pageNumber, int pageSize, int id);
        Task<GetAddendumContractByIdDto> GetAddendumByAddendumContractIdAsync(int id);
        Task<AddendumContract> ApproveRejectAddendum(int contractId, ContractStatus addendumStatus, int addendumId, string empCode);
        Task<AddAddendumContractDto> AddAddendumContractAsync(int id, AddAddendumContractDto addendumContract);
        Task<bool> DeleteAddendumContractAsync(int id);
    }
}
