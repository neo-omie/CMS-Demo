using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedNoticeWithdraw.Command.ApproveNoticeWithdrawal
{
    public class ApproveNoticeWithdrawalCommandHandler : IRequestHandler<ApproveNoticeWithdrawalCommand, ClassifiedContract>
    {
        IClassifiedContractRepository _classifiedContractRepository;
        public ApproveNoticeWithdrawalCommandHandler(IClassifiedContractRepository classifiedContractRepository)
        {
             _classifiedContractRepository = classifiedContractRepository;
        }
        public async Task<ClassifiedContract> Handle(ApproveNoticeWithdrawalCommand request, CancellationToken cancellationToken)
        {
            return await _classifiedContractRepository.ApproveNoticeWithdrawalAsync(request.id, request.empCode, request.status, request.subject, request.emailBody);
        }
    }
}
