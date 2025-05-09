using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotifications(string employeeCode);
        Task<Notification> GetNotificationDetails(int id, string employeeCode);
    }
}
