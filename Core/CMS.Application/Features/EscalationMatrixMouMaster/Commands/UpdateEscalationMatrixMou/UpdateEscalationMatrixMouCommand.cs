using CMS.Application.Features.MasterEscalationMatrixContracts.Command;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou
{
    public record UpdateEscalationMatrixMouCommand(int id, UpdateEscalationMatrixMouDto updateDto) : IRequest<int>;
}
