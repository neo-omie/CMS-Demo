using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ClassifiedPostTermination.Command.ApproveTerminationContract
{
    public class ApproveTerminateContractCommandHandler : IRequestHandler<ApproveTerminateContractCommand, ClassifiedContract>
    {
        readonly IClassifiedContractRepository _classifiedContractRepository;
        public ApproveTerminateContractCommandHandler(IClassifiedContractRepository classifiedContractRepository)
        {
            _classifiedContractRepository = classifiedContractRepository;
        }
        public async Task<ClassifiedContract> Handle(ApproveTerminateContractCommand request, CancellationToken cancellationToken)
        {
            return await _classifiedContractRepository.ApproveTerminateContract(request.id, request.empCode, request.status, request.subject, request.emailBody);
        }
    }
}
