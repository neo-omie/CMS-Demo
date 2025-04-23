using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ContractTypeMaster.Command.DeleteContract
{
   public record DeleteContractCommand(int id):IRequest<bool>;
    
    
}
