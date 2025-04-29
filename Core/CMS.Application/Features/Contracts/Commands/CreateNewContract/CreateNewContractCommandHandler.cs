using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.CreateNewContract
{
    public class CreateNewContractCommandHandler : IRequestHandler<CreateNewContractCommand, Contract>
    {
        readonly IContractRepository _contractRepository;
        readonly IMapper _mapper;
        public CreateNewContractCommandHandler(IContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }
        public async Task<Contract> Handle(CreateNewContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<Contract>(request.cont);
            return await _contractRepository.AddContractAsync(mappedContract);
        }
    }
}
