using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.MasterApostilles.Commands.DeleteApostille
{
    public class DeleteApostilleCommandHandler : IRequestHandler<DeleteApostilleCommand, bool>
    {
        private readonly IMasterApostilleRepository _masterApostilleRepository;

        public DeleteApostilleCommandHandler(IMasterApostilleRepository masterApostilleRepository)
        {
            _masterApostilleRepository = masterApostilleRepository;
        }
        public async Task<bool> Handle(DeleteApostilleCommand request, CancellationToken cancellationToken)
        {
            return await _masterApostilleRepository.DeleteMasterApostilleAsync(request.Id);
        }
    }
}
