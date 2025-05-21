using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.AddendumContracts.Commands.ApproveRejectAddendum
{
    public class ApproveRejectAddendumCommandHandler : IRequestHandler<ApproveRejectAddendumCommand, CMS.Domain.Entities.AddendumContract>
    {
        readonly IAddendumContractRepository _addendumContractRepository;
        readonly IMapper _mapper;
        public ApproveRejectAddendumCommandHandler(IAddendumContractRepository addendumContractRepository, IMapper mapper)
        {
            _addendumContractRepository = addendumContractRepository;
            _mapper = mapper;
        }
        public async Task<Domain.Entities.AddendumContract> Handle(ApproveRejectAddendumCommand request, CancellationToken cancellationToken)
        {
            return await _addendumContractRepository.ApproveRejectAddendum(request.contractId, request.addendumStatus, request.addendumId, request.empCode);
        }
    }
}
