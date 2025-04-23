using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixMOU.Commands.UpdateApprovalMatrixMOU
{
    public class UpdateApprovalMatrixMOUCommandHandler : IRequestHandler<UpdateApprovalMatrixMOUCommand, MasterApprovalMatrixMOU>
    {
        readonly IMasterApprovalMatrixMOURepository _masterApprovalMatrixMOURepository;
        public UpdateApprovalMatrixMOUCommandHandler(IMasterApprovalMatrixMOURepository masterApprovalMatrixMOURepository)
        {
            _masterApprovalMatrixMOURepository = masterApprovalMatrixMOURepository;
        }
        public async Task<MasterApprovalMatrixMOU> Handle(UpdateApprovalMatrixMOUCommand request, CancellationToken cancellationToken)
        {
            return await _masterApprovalMatrixMOURepository.UpdateApprovalMatrixMOU(request.id, request.mou);
        }
    }
}
