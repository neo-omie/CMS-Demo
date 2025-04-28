using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.ApprovalMatrixContract.Commands;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddContractApprovers
{
    public record AddContractApproversCommand(int id, UpdateApprovalMatrixContractDto addApprovers) : IRequest<MasterApprovalMatrixContract>;
}
