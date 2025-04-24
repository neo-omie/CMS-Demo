using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.UpdateContract
{
    public record UpdateContractCommand(int id, UpdateContractDTO ctp): IRequest<ContractTypeMasters>;
 
}
