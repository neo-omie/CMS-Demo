using MediatR;

namespace CMS.Application.Features.MasterApostilles.Commands.DeleteApostille
{
    public record DeleteApostilleCommand(int Id) : IRequest<bool>;
}
