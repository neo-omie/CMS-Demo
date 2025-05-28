using CMS.Domain.Entities;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;



namespace CMS.Application.Features.MasterApostilles.Commands.AddApostille
{
    public record AddApostilleCommand(AddApostilleDto ApostilleDTO,string empCode) : IRequest<CMS.Domain.Entities.MasterApostille>;
    
}
