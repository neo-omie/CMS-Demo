using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.Contracts.Commands.ApproveRejectContract;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.ApproveRejectContract
{
    public class ApproveRejectClassifiedContractHandler : IRequestHandler<ApproveRejectClassifiedContractCommand, ClassifiedContract>
    {
        readonly IClassifiedContractRepository _contractRepository;
        public  ApproveRejectClassifiedContractHandler(IClassifiedContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
            }
        public async Task<ClassifiedContract> Handle(ApproveRejectClassifiedContractCommand request, CancellationToken cancellationToken)
        {
            return await _contractRepository.ApproveRejectContract(request.id, request.empCode, request.status);
        }

    }
}
