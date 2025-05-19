using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.NoticeWithdraw.Command.AddNoticeWithdrawalDetails;
using CMS.Domain.Constants;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface INoticeWithdrawalRepository
    {
        Task<bool> AddNoticeWithdrawalDetailsAsync(int contractId, int postTermId, NoticeWithdrawalDocumentUploadDto noticeWithdrawalDocumentUploadDto);
        public Task<Contract> ApproveNoticeWithdrawalAsync(int id, string empCode, ContractStatus status, string subject, string emailBody);
    }
}
