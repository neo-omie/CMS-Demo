using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Application.Exceptions;
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
        public async Task<IEnumerable<Notification>> GetAllNotifications(string employeeCode)
        {
            var allNotifs = await _context.ContractNotifications.Where(cn => cn.EmployeeCode == employeeCode).ToListAsync();
            if(allNotifs == null)
            {
                throw new NotFoundException("No Notifications found currently");
            }
            return allNotifs;
        }

        public async Task<Notification> GetNotificationDetails(int id, string employeeCode)
        {
            var notif = await _context.ContractNotifications.FirstOrDefaultAsync(cn => (cn.EmployeeCode == employeeCode) && (cn.ValueId == id));
            if(notif == null)
            {
                throw new NotFoundException("Notification not found");
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
    }
}
