using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Queries.GetAllContracts
{
    public record GetAllContractsQuery(FiltersContractDto filters,string eCode) : IRequest<IEnumerable<GetAllContractsDto>>;
}
