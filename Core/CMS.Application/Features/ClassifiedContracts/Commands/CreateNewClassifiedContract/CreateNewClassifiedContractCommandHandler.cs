using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.ClassifiedContracts.Commands.CreateNewContract
{
    public class CreateNewClassifiedContractCommandHandler : IRequestHandler<CreateNewClassifiedContractCommand, ClassifiedContract>
    {
        readonly IClassifiedContractRepository _contractRepository;
        readonly IMapper _mapper;
        public CreateNewClassifiedContractCommandHandler(IClassifiedContractRepository contractRepository, IMapper mapper)
        {   
            _contractRepository = contractRepository;
            _mapper = mapper;
        }
        public async Task<ClassifiedContract> Handle(CreateNewClassifiedContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<ClassifiedContract>(request.cont);
            return await _contractRepository.AddClassifiedContractAsync(mappedContract);
        }
    }
}
