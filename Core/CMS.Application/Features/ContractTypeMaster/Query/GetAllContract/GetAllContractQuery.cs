using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Query.GetAllContract
{
    public record GetAllContractQuery(int pageNumber, int pageSize):IRequest<IEnumerable<ContractTypeMasters>>;
    
}
