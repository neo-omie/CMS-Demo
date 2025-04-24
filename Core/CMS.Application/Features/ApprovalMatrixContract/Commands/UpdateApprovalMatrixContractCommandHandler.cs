using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ApprovalMatrixContract.Commands
{
    public class UpdateApprovalMatrixContractCommandHandler : IRequestHandler<UpdateApprovalMatrixContractCommand, bool>
    {
        readonly IMasterApprovalMatrixContractRepository _masterApprovalMatrixContractRepository;
        public UpdateApprovalMatrixContractCommandHandler(IMasterApprovalMatrixContractRepository masterApprovalMatrixContractRepository)
        {
            _masterApprovalMatrixContractRepository = masterApprovalMatrixContractRepository;
        }
        public async Task<bool> Handle(UpdateApprovalMatrixContractCommand request, CancellationToken cancellationToken)
        {
            return await _masterApprovalMatrixContractRepository.UpdateApprovalMatrixContract(request.id, request.contract);
        }
    }
}
