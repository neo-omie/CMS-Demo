using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.PostTermination.Command.AddCommand
{
    public class AddPostTerminationCommandHandler : IRequestHandler<AddPostTerminationCommand, bool>
    {
        readonly IPostTerminationRepository _postTerminationRepository;
        public AddPostTerminationCommandHandler(IPostTerminationRepository postTerminationRepository)
        {
            _postTerminationRepository = postTerminationRepository;
        }
        public async Task<bool> Handle(AddPostTerminationCommand request, CancellationToken cancellationToken)
        {
            return await _postTerminationRepository.AddTerminationDetailsAsync(request.contractId, request.modelDto);
        }
    }
}
