using CMS.Domain.Constants;

namespace CMS.Application.Services
{
    public interface IAuditLogService
    {
        Task LogActionAsync(string action, TableList tableName, int recordId, string oldValue, string newValue, string loggedBy);
    }
}
