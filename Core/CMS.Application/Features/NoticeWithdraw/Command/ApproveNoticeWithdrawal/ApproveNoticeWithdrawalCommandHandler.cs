using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.NoticeWithdraw.Command.ApproveNoticeWithdrawal
{
    public class ApproveNoticeWithdrawalCommandHandler : IRequestHandler<ApproveNoticeWithdrawalCommand, Contract>
    {
        readonly INoticeWithdrawalRepository _noticeWithdrawalRepository;
        public ApproveNoticeWithdrawalCommandHandler(INoticeWithdrawalRepository noticeWithdrawalRepository)
        {
            _noticeWithdrawalRepository = noticeWithdrawalRepository;
        }
        public async Task<Contract> Handle(ApproveNoticeWithdrawalCommand request, CancellationToken cancellationToken)
        {
            return await _noticeWithdrawalRepository.ApproveNoticeWithdrawalAsync(request.id, request.empCode, request.status, request.subject, request.emailBody);
        }
    }
}
