using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou
{
    public class UpdateEscalationMatrixMouCommandHandler : IRequestHandler<UpdateEscalationMatrixMouCommand, bool>
    {
        private readonly IMasterEscalationMatrixMouRepository _mouRepository;

        public UpdateEscalationMatrixMouCommandHandler(IMasterEscalationMatrixMouRepository mouRepository)
        {
            _mouRepository = mouRepository;

        }
        public Task<bool> Handle(UpdateEscalationMatrixMouCommand request, CancellationToken cancellationToken)
        {
            return _mouRepository.UpdateMatrixMou(request.id,request.updateDto,request.empCode);
        }
    }
}
