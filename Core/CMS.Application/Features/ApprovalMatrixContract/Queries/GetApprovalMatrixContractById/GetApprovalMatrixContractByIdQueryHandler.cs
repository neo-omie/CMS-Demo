using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById
{
    public class GetApprovalMatrixContractByIdQueryHandler : IRequestHandler<GetApprovalMatrixContractByIdQuery, GetApprovalMatrixContractByIdDto>
    {
        private readonly IMasterApprovalMatrixContractRepository _masterApprovalMatrixContractRepository;
        public GetApprovalMatrixContractByIdQueryHandler(IMasterApprovalMatrixContractRepository masterApprovalMatrixContractRepository, IMapper mapper)
        {
            _masterApprovalMatrixContractRepository = masterApprovalMatrixContractRepository;
        }
        public async Task<GetApprovalMatrixContractByIdDto> Handle(GetApprovalMatrixContractByIdQuery request, CancellationToken cancellationToken)
        {
            return await _masterApprovalMatrixContractRepository.GetApprovalMatrixContractById(request.id);
        }
    }
}
