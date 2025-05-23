//using CMS.Domain.Constants;
//using CMS.Domain.Entities;
//using CMS.Persistence.Context;

//namespace  CMS.Application.Services
//{
//    public class AuditLogService : IAuditLogService
//    {
//        private readonly CMSDbContext _context;

//        public AuditLogService(CMSDbContext context)
//        {
//            _context = context;
//        }

//        public async Task LogActionAsync(string action, TableList tableName, int recordId, string oldValue, string newValue, string loggedBy)
//        {
//            var auditTrail = new AuditTrail
//            {
//                ActionDescription = $"{action} record with ID: {recordId}. Old values: {oldValue}, New values: {newValue}",
//                ForTable = tableName,
//                TableId = recordId,
//                LogTime = DateTime.UtcNow,
//                LoggedBy = loggedBy,
//                Status = GetLogStatusForAction(action)
//            };

//            _context.AuditTrails.Add(auditTrail);
//            await _context.SaveChangesAsync();
//        }
//        private LogStatus GetLogStatusForAction(string action)
//        {
//            return action.ToLower() switch
//            {
//                "create" => LogStatus.Created,
//                "update" => LogStatus.Updated,
//                "delete" => LogStatus.Deleted,
//                "approve" => LogStatus.Approved,
//                "reject" => LogStatus.Rejected,
//                _ => throw new ArgumentException($"Invalid action: {action}")
//            };
//        }
//    }
//}
