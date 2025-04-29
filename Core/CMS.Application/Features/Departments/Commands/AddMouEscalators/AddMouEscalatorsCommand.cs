using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Departments.Commands.AddMouEscalators
{
    public record AddMouEscalatorsCommand(int id, UpdateEscalationMatrixMouDto addEscalators) : IRequest<MasterEscalationMatrixMou>;
}
