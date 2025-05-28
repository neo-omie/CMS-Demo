using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;
using CMS.Application.Features.AddendumContract.AddendumContractDto;

namespace CMS.Application.Features.AddendumContracts.Commands.AddAddendumContract
{
    public class AddAddendumContractCommandHandler:IRequestHandler<AddAddendumContractCommand, AddAddendumContractDto>
    {
        readonly IAddendumContractRepository _addendumContractRepository;
        readonly IMapper _mapper;
        public AddAddendumContractCommandHandler(IAddendumContractRepository addendumContractRepository, IMapper mapper)
        {
            _addendumContractRepository = addendumContractRepository;
            _mapper = mapper;
        }
        public async Task<AddAddendumContractDto> Handle(AddAddendumContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<AddAddendumContractDto>(request.addendumDto);
            return await _addendumContractRepository.AddAddendumContractAsync(request.id, mappedContract,request.empCode);
        }
    }
}
