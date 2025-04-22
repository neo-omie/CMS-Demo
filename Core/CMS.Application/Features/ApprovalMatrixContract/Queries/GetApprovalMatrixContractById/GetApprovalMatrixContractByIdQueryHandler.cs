using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById
{
    public class GetApprovalMatrixContractByIdQueryHandler : IRequestHandler<GetApprovalMatrixContractByIdQuery, GetApprovalMatrixContractByIdDto>
    {
        public Task<GetApprovalMatrixContractByIdDto> Handle(GetApprovalMatrixContractByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
