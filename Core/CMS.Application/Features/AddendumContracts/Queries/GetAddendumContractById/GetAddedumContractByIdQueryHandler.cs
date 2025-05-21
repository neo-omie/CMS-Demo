using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.AddendumContractDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Features.AddendumContracts.Queries.GetAddendumContractById
{
    public class GetAddedumContractByIdQueryHandler : IRequestHandler<GetAddedumContractByIdQuery, GetAddendumContractByIdDto>
    {
        private readonly IAddendumContractRepository _addendumContractRepository;
        private readonly IMapper _mapper;

        public GetAddedumContractByIdQueryHandler(IAddendumContractRepository addendumContractRepository, IMapper mapper)
        {
            _addendumContractRepository = addendumContractRepository;
            _mapper = mapper;
        }

        public async Task<GetAddendumContractByIdDto> Handle(GetAddedumContractByIdQuery request, CancellationToken cancellationToken)
        {
            var gotAddendum= await _addendumContractRepository.GetAddendumByAddendumContractIdAsync(request.id);
            //var mappedAddendum= _mapper.Map<AddAddendumContractDto>(gotAddendum);
            return gotAddendum;
        }
    }
}
