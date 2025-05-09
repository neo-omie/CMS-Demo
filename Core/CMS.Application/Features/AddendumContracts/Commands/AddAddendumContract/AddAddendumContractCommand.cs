using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using CMS.Domain.Entities;
using CMS.Application.Features.AddendumContract.AddendumContractDto;

namespace CMS.Application.Features.AddendumContracts.Commands.AddAddendumContract
{
    public  record AddAddendumContractCommand(int id, AddAddendumContractDto addendumDto):IRequest<Domain.Entities.AddendumContract>;
}
