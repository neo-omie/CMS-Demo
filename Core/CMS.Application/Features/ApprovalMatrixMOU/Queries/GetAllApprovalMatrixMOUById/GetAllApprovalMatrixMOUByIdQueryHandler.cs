using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById
{
    public class GetAllApprovalMatrixMOUByIdQueryHandler : IRequestHandler<GetAllApprovalMatrixMOUByIdQuery, GetAllApprovalMatrixMOUByIdDto>
    {
        readonly IMasterApprovalMatrixMOURepository _masterApprovalMatrixMOURepository;
        public GetAllApprovalMatrixMOUByIdQueryHandler(IMasterApprovalMatrixMOURepository masterApprovalMatrixMOURepository)
        {
            _masterApprovalMatrixMOURepository = masterApprovalMatrixMOURepository;
        }
        public async Task<GetAllApprovalMatrixMOUByIdDto> Handle(GetAllApprovalMatrixMOUByIdQuery request, CancellationToken cancellationToken)
        {
            return await _masterApprovalMatrixMOURepository.GetApprovalMatrixMOUById(request.id);
        }
    }
}
