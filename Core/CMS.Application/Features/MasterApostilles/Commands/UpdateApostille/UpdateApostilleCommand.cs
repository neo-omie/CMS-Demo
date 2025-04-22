using CMS.Domain.Entities;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Commands.UpdateApostille
{
    public record UpdateApostilleCommand(int id, UpdateApostilleDto apostilleDTO) : IRequest<CMS.Domain.Entities.MasterApostille>;
}
