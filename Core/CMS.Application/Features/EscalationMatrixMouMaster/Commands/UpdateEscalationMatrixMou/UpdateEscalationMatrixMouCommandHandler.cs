using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou
{
    public class UpdateEscalationMatrixMouCommandHandler : IRequestHandler<UpdateEscalationMatrixMouCommand, int>
    {
        private readonly IMasterEscalationMatrixMouRepository _mouRepository;

        public UpdateEscalationMatrixMouCommandHandler(IMasterEscalationMatrixMouRepository mouRepository)
        {
            _mouRepository = mouRepository;

        }
        public Task<int> Handle(UpdateEscalationMatrixMouCommand request, CancellationToken cancellationToken)
        {
            return _mouRepository.UpdateMatrixMou(request.id,request.updateDto);
        }
    }
}
