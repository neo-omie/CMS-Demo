using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.MasterEscalationMatrixContracts;
using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEscalationMatrixContractRepository
    {
        Task<int> UpdateMatrixContract(int valueId,UpdateEscalationMatrixContractDto updateDto);

        Task<GetEscalationMatrixContractDto> GetEscalationMatrixContract(int valueId);


        Task<(IEnumerable<GetEscalationMatrixContractDto>, int)> GetAllEscalationMatrixContract(int pageNumber, int pageSize);
    }
}
