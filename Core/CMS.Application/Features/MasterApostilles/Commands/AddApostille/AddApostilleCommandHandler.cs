using CMS.Domain.Entities;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using MediatR;
using AutoMapper;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Features.MasterApostilles.Commands.AddApostille;

namespace CMS.Application.Features.MasterApostilles.Commands.AddApostille
{
    public class AddApostilleCommandHandler : IRequestHandler<AddApostilleCommand, MasterApostille>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        private readonly IMapper _mapper;

        public AddApostilleCommandHandler(IMasterApostilleRepository masterApostilleRepository, IMapper mapper)
        {
            _masterApostilleRepository = masterApostilleRepository;
            _mapper = mapper;
        }
        public async Task<MasterApostille> Handle(AddApostilleCommand request, CancellationToken cancellationToken)
        {
            var apostille= _mapper.Map<MasterApostille>(request.ApostilleDTO);
            return await _masterApostilleRepository.AddMasterApostilleAsync(apostille);
        }
    }
}
