using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterDocuments.Queries.GetAllDocument;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterEscalationMatrixContracts.Queries.GetAllEscalationMatrixContracts
{
    public class GetAllEscalationMatrixContractHandler : IRequestHandler<GetAllEscalationMatrixContractQuery, (IEnumerable<GetEscalationMatrixContractDto>, int)>
    {
        private readonly IMasterEscalationMatrixContractRepository _contractRepository;

        public GetAllEscalationMatrixContractHandler(IMasterEscalationMatrixContractRepository contractRepository)
        {
           _contractRepository = contractRepository;

        }
        public Task<(IEnumerable<GetEscalationMatrixContractDto>, int)> Handle(GetAllEscalationMatrixContractQuery request, CancellationToken cancellationToken)
        {

           return _contractRepository.GetAllEscalationMatrixContract(request.pageNumber,request.pageSize);
        }
    }
}
