using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApostilleRepository
    {
        Task<IEnumerable<MasterApostille>> GetAllMasterApostilleAsync(string unit, string searchTerm, int pageNumber, int pageSize);
        Task<MasterApostille> GetMasterApostilleByIdAsync(int id);
        Task<MasterApostille> AddMasterApositlleAsync(MasterApostille masterApostille);
        Task<MasterApostille> UpdateMasterApostilleAsync(int id, MasterApostille masterApostille);
        Task<bool> DeleteMasterApostilleAsync(int id);
    }
}
