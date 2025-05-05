using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.RemoveClassifiedContract
{
    public class RemoveClassifiedContractCommandHandler : IRequestHandler<RemoveClassifiedContractCommand, bool>
    {
        readonly IClassifiedContractRepository _contractRepository;
        public RemoveClassifiedContractCommandHandler(IClassifiedContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }
        public async Task<bool> Handle(RemoveClassifiedContractCommand request, CancellationToken cancellationToken)
        {
            return await _contractRepository.DeleteClassifiedContractAsync(request.id);
        }
    }
}
