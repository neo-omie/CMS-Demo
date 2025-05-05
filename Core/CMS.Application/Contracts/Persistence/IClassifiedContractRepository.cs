using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IClassifiedContractRepository
    {
        Task<IEnumerable<GetAllClassifiedContractsDto>> GetAllClassifiedContractsAsync(int pageNumber, int pageSize);
        Task<GetClassifiedContractByIdDto> GetClassifiedContractByIdAsync(int id);
        Task<ClassifiedContract> AddClassifiedContractAsync(ClassifiedContract cp);
        Task<bool> UpdateClassifiedContractAsync(int id, ClassifiedContract cp);
        Task<bool> DeleteClassifiedContractAsync(int id);
    }
}
