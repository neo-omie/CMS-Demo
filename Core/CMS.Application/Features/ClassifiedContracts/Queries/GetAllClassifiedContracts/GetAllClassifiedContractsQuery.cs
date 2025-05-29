using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts
{
    public record GetAllClassifiedContractsQuery(FiltersContractDto filterContractDto) : IRequest<IEnumerable<GetAllClassifiedContractsDto>>;
}
