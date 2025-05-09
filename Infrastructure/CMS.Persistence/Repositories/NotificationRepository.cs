using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;

namespace CMS.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        readonly CMSDbContext _context;
        public NotificationRepository(CMSDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Notification>> GetAllNotifications(string employeeCode)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetNotificationDetails(int id, string employeeCode)
        {
            throw new NotImplementedException();
        }
    }
}
