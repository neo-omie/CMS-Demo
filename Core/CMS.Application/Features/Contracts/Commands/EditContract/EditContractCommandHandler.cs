using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Contracts.Commands.EditContract
{
    public class EditContractCommandHandler : IRequestHandler<EditContractCommand, bool>
    {
        readonly IContractRepository _contractRepository;
        readonly IMapper _mapper;
        public EditContractCommandHandler(IContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(EditContractCommand request, CancellationToken cancellationToken)
        {
            var mappedContract = _mapper.Map<Contract>(request.cont);
            return await _contractRepository.UpdateContractAsync(request.id, mappedContract);
        }
    }
}
