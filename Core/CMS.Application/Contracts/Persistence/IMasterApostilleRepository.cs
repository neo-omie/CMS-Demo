using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface IMasterApostilleRepository
    {
        Task<(IEnumerable<MasterApostille> Data, int TotalCount)> GetAllMasterApostilleAsync(int pageNumber, int pageSize, string? searchTerm);
        Task<MasterApostille> GetMasterApostilleByIdAsync(int id);
        Task<MasterApostille> AddMasterApostilleAsync(MasterApostille masterApostille, string empCode);
        Task<MasterApostille> UpdateMasterApostilleAsync(int id, MasterApostille masterApostille, string empCode);
        Task<bool> DeleteMasterApostilleAsync(int id,string empCode);
    }
}
