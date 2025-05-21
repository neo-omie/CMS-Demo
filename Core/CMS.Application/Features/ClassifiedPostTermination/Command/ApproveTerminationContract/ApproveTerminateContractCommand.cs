using CMS.Domain.Constants;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ClassifiedPostTermination.Command.ApproveTerminationContract
{
    public record ApproveTerminateContractCommand(int id , string empCode,ContractStatus status,string subject , string emailBody):IRequest<ClassifiedContract>;
    
}
