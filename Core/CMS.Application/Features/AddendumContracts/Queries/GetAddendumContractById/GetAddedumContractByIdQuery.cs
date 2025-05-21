using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.AddendumContractDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAddendumContractById
{
    public record GetAddedumContractByIdQuery(int id):IRequest<GetAddendumContractByIdDto>;
}
