using CMS.Application.Features.EscalationMatrixMouMaster;
using CMS.Application.Features.EscalationMatrixMouMaster.Commands.UpdateEscalationMatrixMou;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterEscalationMatrixMouRepository
    {
        public Task<IEnumerable<EscalationMatrixMoutDto>> GetAllEscalationMatrixMou(int pageNumber, int pageSize);
        public Task<EscalationMatrixMoutDto> GetEscalationMatrixMou(int valueId);
        public Task<int> UpdateMatrixMou(int valueId, UpdateEscalationMatrixMouDto updateDto);
    }
}
