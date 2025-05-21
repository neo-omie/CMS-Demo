using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAllAddendumContracts
{
    public record GetAllAddendumContractsQuery(int pageNumber, int pageSize, DateTime? searchTerm):IRequest<object>;
}
