using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.AddendumContracts.Commands.AddAddendumContract
{
    public class AddAddendumContractCommandHandler:IRequestHandler<AddAddendumContractCommand, Domain.Entities.AddendumContract>
    {
        readonly IAddendumContractRepository _addendumContractRepository;
        readonly IMapper _mapper;
        public AddAddendumContractCommandHandler(IAddendumContractRepository addendumContractRepository, IMapper mapper)
        {
            _addendumContractRepository = addendumContractRepository;
            _mapper = mapper;
        }
        public async Task<Domain.Entities.AddendumContract> Handle(AddAddendumContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<Domain.Entities.AddendumContract>(request.addendumDto);
            return await _addendumContractRepository.AddAddendumContractAsync(request.id, mappedContract);
        }
    }
}
