using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddContractApprovers
{
    public class AddContractApproversCommandHandler : IRequestHandler<AddContractApproversCommand, MasterApprovalMatrixContract>
    {
        readonly IDepartmentRepository _departmentRepository;
        public AddContractApproversCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<MasterApprovalMatrixContract> Handle(AddContractApproversCommand request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.AddContractApprovers(request.id, request.addApprovers);
        }
    }
}
