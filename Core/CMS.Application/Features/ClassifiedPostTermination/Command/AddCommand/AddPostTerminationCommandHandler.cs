using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.ClassifiedPostTermination.Command.AddCommand
{
    public class AddPostTerminationCommandHandler : IRequestHandler<AddPostTerminationCommand, bool>
    {
        readonly IClassifiedContractRepository _classifiedContractRepository;
        public AddPostTerminationCommandHandler(IClassifiedContractRepository classifiedContractRepository)
        {
            _classifiedContractRepository = classifiedContractRepository;
        }
        public async Task<bool> Handle(AddPostTerminationCommand request, CancellationToken cancellationToken)
        {
            return await _classifiedContractRepository.AddTerminationDetailsAsync(request.contractId, request.modelDto);
        }
    }
}
