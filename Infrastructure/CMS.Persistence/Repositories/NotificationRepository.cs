using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
using CMS.Application.Features.Notifications.Queries.GetAllNotifications;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        readonly CMSDbContext _context;
        public NotificationRepository(CMSDbContext context)
        {
            _context = context;
        }
        public async  Task<IEnumerable<GetAllNotificationsDto>> GetAllNotifications(int pageNumber,int pageSize ,string employeeCode)
        {
            //var allNotifs = await _context.ContractNotifications.Where(cn => cn.EmployeeCode == employeeCode && cn.isDeleted == false).OrderByDescending(cn => cn.NotificationDate).ToListAsync();

            string query = " EXEC SP_GetAllNotifications @pageNumber={0} ,@pageSize={1},@employeeCode={2}";
            var allNotifs =  _context.ContractNotificationsDto.FromSqlRaw(query, pageNumber, pageSize, employeeCode); 
            if (allNotifs == null)
            {
                throw new NotFoundException("No Notifications found currently");
            }
            //await allNotifs.ForEachAsync(n => { n.totalRecords =  _context.ContractNotifications.Where(cn => cn.EmployeeCode == employeeCode && cn.isDeleted == false).Count(); });
            return allNotifs;
        }

        public async Task<Notification> GetNotificationDetails(int id, string employeeCode)
        {
            var notif = await _context.ContractNotifications.FirstOrDefaultAsync(cn => (cn.EmployeeCode == employeeCode) && (cn.ValueId == id) && (cn.isDeleted == false));
            if(notif == null)
            {
                throw new NotFoundException("Notification not found");
            }
            if(notif.isRead == false)
            {
                notif.isRead = true;
                _context.ContractNotifications.Update(notif);
                await _context.SaveChangesAsync();
            }
            return notif;
        }
        public async Task<bool> NewNotification(Notification notification)
        {
            var addNewNotif = await _context.ContractNotifications.AddAsync(notification);
            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            throw new Exception("For some reasons, notification not added.");
        }
        public async Task<int> UnreadNotificationsCount(string employeeCode)
        {
            var allNotifs = await _context.ContractNotifications.Where(cn => cn.isRead == false && cn.EmployeeCode == employeeCode && cn.isDeleted == false).CountAsync();
            return allNotifs;
        }

        
    }
}
