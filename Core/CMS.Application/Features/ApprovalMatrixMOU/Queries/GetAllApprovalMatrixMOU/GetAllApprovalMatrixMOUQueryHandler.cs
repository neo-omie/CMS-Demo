using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU
{
    public class GetAllApprovalMatrixMOUQueryHandler : IRequestHandler<GetAllApprovalMatrixMOUQuery, IEnumerable<GetAllApprovalMatrixMOUDto>>
    {
        readonly IMasterApprovalMatrixMOURepository _masterApprovalMatrixMOURepository;
        public GetAllApprovalMatrixMOUQueryHandler(IMasterApprovalMatrixMOURepository masterApprovalMatrixMOURepository)
        {
            _masterApprovalMatrixMOURepository = masterApprovalMatrixMOURepository;
        }
        public async Task<IEnumerable<GetAllApprovalMatrixMOUDto>> Handle(GetAllApprovalMatrixMOUQuery request, CancellationToken cancellationToken)
        {
            return await _masterApprovalMatrixMOURepository.GetAllApprovalMatrixMOU(request.pageNumber, request.pageSize);
        }
    }
}
