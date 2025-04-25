using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddContractEscalators
{
    public class AddContractEscalatorsCommandHandler : IRequestHandler<AddContractEscalatorsCommand, MasterEscalationMatrixContract>
    {
        readonly IDepartmentRepository _departmentRepository;
        public AddContractEscalatorsCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<MasterEscalationMatrixContract> Handle(AddContractEscalatorsCommand request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.AddContractEscalators(request.id, request.addEscalators);
        }
    }
}
