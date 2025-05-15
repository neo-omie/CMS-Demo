using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.PostTermination.Command.ApproveTerminationContract
{
    public class ApproveTerminateContractCommandHandler : IRequestHandler<ApproveTerminateContractCommand, Contract>
    {
        readonly IPostTerminationRepository _postTerminationRepository;
        public ApproveTerminateContractCommandHandler(IPostTerminationRepository postTerminationRepository)
        {
            _postTerminationRepository = postTerminationRepository;
        }
        public async Task<Contract> Handle(ApproveTerminateContractCommand request, CancellationToken cancellationToken)
        {
            return await _postTerminationRepository.ApproveTerminateContract(request.id, request.empCode, request.status, request.subject, request.emailBody);
        }
    }
}
