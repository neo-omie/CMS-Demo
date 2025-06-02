using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Features.Notifications.Queries.GetAllNotifications;
using CMS.Domain.Entities;

namespace CMS.Application.Contracts.Persistence
{
    public interface INotificationRepository
    {
        //Task<IEnumerable<Notification>> GetAllNotifications(string employeeCode);
        //Task<IEnumerable<Notification>> GetAllNotifications(int pageNumber,int pageSize ,string employeeCode);
        Task<IEnumerable<GetAllNotificationsDto>> GetAllNotifications(int pageNumber,int pageSize ,string employeeCode);
        Task<Notification> GetNotificationDetails(int id, string employeeCode);
        Task<bool> NewNotification(Notification notification);
        Task<int> UnreadNotificationsCount(string employeeCode);
    }
}
