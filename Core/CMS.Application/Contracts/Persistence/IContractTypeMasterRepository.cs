using CMS.Application.Features.ContractTypeMaster.Query;
using CMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Contracts.Persistence
{
   public  interface IContractTypeMasterRepository
    {
        Task<IEnumerable<GetAllContractTypesDTO>> GetAllContractAsync( int pageNumber, int pageSize);

        Task<ContractTypeMasters> GetContractById(int id);

        Task<ContractTypeMasters> AddContractAsync(ContractTypeMasters ctp,string empCode);

        Task<ContractTypeMasters> UpdateContractAsync(int id, ContractTypeMasters ctp, string empCode);

        Task<bool> DeletContract(int id, string empCode);
    }
}
