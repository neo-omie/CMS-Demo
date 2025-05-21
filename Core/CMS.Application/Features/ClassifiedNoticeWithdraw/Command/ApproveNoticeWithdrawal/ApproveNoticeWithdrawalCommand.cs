using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedNoticeWithdraw.Command.ApproveNoticeWithdrawal
{
    public record ApproveNoticeWithdrawalCommand(int id, string empCode, ContractStatus status, string subject, string emailBody) : IRequest<ClassifiedContract>;
}
