using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails
{
    public record AddNoticeWithdrawalDetailsCommand(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto) : IRequest<bool>;
}
