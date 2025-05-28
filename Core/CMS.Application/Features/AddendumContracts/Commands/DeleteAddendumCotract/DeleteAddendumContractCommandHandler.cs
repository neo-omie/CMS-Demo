using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.AddendumContracts.Commands.DeleteAddendumCotract
{
    public class DeleteAddendumContractCommandHandler:IRequestHandler<DeleteAddendumContractCommand, bool>
    {
        private readonly IAddendumContractRepository _addendumContractRepository;

        public DeleteAddendumContractCommandHandler(IAddendumContractRepository addendumContractRepository)
        {
            _addendumContractRepository = addendumContractRepository;
        }
        public async Task<bool> Handle(DeleteAddendumContractCommand request, CancellationToken cancellationToken)
        {
            await _addendumContractRepository.DeleteAddendumContractAsync(request.AddendumId,request.empCode);
            return true;
        }
    }
}
