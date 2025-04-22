using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.EscalationMatrixContract;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEscalationMatrixContractRepository
    {
        Task<int> UpdateMatrixContract(int valueId);

        Task<MasterEscalationMatrixContract> GetEscalationMatrixContract(int valueId);


        Task<IEnumerable<GetEscalationMatrixContractDto>> GetAllEscalationMatrixContract(int pageNumber, int pageSize);
    }
}
