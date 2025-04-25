using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Departments.Commands.AddContractApprovers;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddMOUApprovers
{
    public class AddMOUApproversCommandHandler : IRequestHandler<AddMOUApproversCommand, MasterApprovalMatrixMOU>
    {
        readonly IDepartmentRepository _departmentRepository;
        public AddMOUApproversCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<MasterApprovalMatrixMOU> Handle(AddMOUApproversCommand request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.AddMOUApprovers(request.id, request.addApprovers);
        }
    }
}
