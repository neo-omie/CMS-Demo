using CMS.Domain.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.AddendumContracts.Commands.ApproveRejectAddendum
{
    public record ApproveRejectAddendumCommand(int contractId, ContractStatus addendumStatus, int addendumId, string empCode) :IRequest<CMS.Domain.Entities.AddendumContract>;
}
