using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using MediatR;

namespace CMS.Application.Features.ClassifiedNoticeWithdraw.Command.AddNoticeWithdrawalDetails
{
    public record AddNoticeWithdrawalDetailsCommand(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto) : IRequest<bool>;
}
