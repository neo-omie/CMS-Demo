using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.RemoveContract
{
    public class RemoveContractCommandHandler : IRequestHandler<RemoveContractCommand, bool>
    {
        readonly IContractRepository _contractRepository;
        public RemoveContractCommandHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<bool> Handle(RemoveContractCommand request, CancellationToken cancellationToken)
        {
            return await _contractRepository.DeleteContractAsync(request.id);
        }
    }
}
