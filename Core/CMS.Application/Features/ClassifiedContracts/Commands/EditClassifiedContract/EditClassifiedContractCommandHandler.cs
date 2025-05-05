using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.EditClassifiedContract
{
    public class EditClassifiedContractCommandHandler : IRequestHandler<EditClassifiedContractCommand, bool>
    {
        readonly IClassifiedContractRepository _contractRepository;
        readonly IMapper _mapper;
        public EditClassifiedContractCommandHandler(IClassifiedContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(EditClassifiedContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<ClassifiedContract>(request.cont);
            return await _contractRepository.UpdateClassifiedContractAsync(request.id, mappedContract);
        }
    }
}
