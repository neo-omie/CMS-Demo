using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails
{
    public class AddNoticeWithdrawalDetailsCommandHandler : IRequestHandler<AddNoticeWithdrawalDetailsCommand, bool>
    {
        readonly INoticeWithdrawalRepository _noticeWithdrawalRepository;
        public AddNoticeWithdrawalDetailsCommandHandler(INoticeWithdrawalRepository noticeWithdrawalRepository)
        {
            _noticeWithdrawalRepository = noticeWithdrawalRepository;
        }
        public async Task<bool> Handle(AddNoticeWithdrawalDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _noticeWithdrawalRepository.AddNoticeWithdrawalDetailsAsync(request.contractId, request.postTermId, request.noticeWithdrawalDocumentUploadDto);
        }
    }
}
