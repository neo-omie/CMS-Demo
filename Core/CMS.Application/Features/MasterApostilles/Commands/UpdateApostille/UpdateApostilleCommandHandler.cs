using AutoMapper;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Commands.UpdateApostille
{
    public class UpdateApostilleCommandHandler:IRequestHandler<UpdateApostilleCommand, CMS.Domain.Entities.MasterApostille>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;
        private readonly IMapper _mapper;

        public UpdateApostilleCommandHandler(IMasterApostilleRepository masterApostilleRepository, IMapper mapper)
        {
            _masterApostilleRepository = masterApostilleRepository;
            _mapper = mapper;
        }
        public Task<CMS.Domain.Entities.MasterApostille> Handle(UpdateApostilleCommand request, CancellationToken cancellationToken)
        {
            var apostille= _mapper.Map<CMS.Domain.Entities.MasterApostille>(request.apostilleDTO);
            return _masterApostilleRepository.UpdateMasterApostilleAsync(request.id, apostille);
        }
    }
}
