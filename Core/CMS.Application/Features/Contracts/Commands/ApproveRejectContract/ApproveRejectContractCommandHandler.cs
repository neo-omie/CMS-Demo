using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.ApproveRejectContract
{
    public class ApproveRejectContractCommandHandler : IRequestHandler<ApproveRejectContractCommand, Contract>
    {
            readonly IContractRepository _contractRepository;
            public ApproveRejectContractCommandHandler(IContractRepository contractRepository)
            {
                _contractRepository = contractRepository;
            }
            public async Task<Contract> Handle(ApproveRejectContractCommand request, CancellationToken cancellationToken)
            {
                return await _contractRepository.ApproveRejectContract(request.id, request.empCode, request.status);
            }
        
    }
}
