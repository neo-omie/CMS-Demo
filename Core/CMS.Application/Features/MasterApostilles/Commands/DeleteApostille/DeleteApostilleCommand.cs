using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CMS.Application.Features.MasterApostilles.Commands.DeleteApostille
{
    public record DeleteApostilleCommand(int Id,string empCode) : IRequest<bool>;
}
