using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddMouEscalators
{
    public class AddMouEscalatorsCommandHandler : IRequestHandler<AddMouEscalatorsCommand, MasterEscalationMatrixMou>
    {
        readonly IDepartmentRepository _departmentRepository;
        public AddMouEscalatorsCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public async Task<MasterEscalationMatrixMou> Handle(AddMouEscalatorsCommand request, CancellationToken cancellationToken)
        {
            return await _departmentRepository.AddMouEscalators(request.id, request.addEscalators);
        }
    }
}
