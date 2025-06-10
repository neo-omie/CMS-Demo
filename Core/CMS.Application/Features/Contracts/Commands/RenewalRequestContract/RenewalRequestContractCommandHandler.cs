using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Commands.RemoveContract;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.RenewalRequestContract
{
    public class RenewalRequestContractCommandHandler : IRequestHandler<RenewalRequestContractCommand, Contract>
    {
        readonly IContractRepository _contractRepository;
        public RenewalRequestContractCommandHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        Task<Contract> IRequestHandler<RenewalRequestContractCommand, Contract>.Handle(RenewalRequestContractCommand request, CancellationToken cancellationToken)
        {
            return _contractRepository.RenewalRequestContractAsync(request.id, request.empCode);
        }
    }
}
