using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Queries.GetApostilleById
{
    public record GetApostilleByIdQuery(int Id) : IRequest<CMS.Domain.Entities.MasterApostille>;
}