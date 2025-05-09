using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Notifications.Queries.GetAllNotifications
{
    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IEnumerable<Notification>>
    {
        readonly INotificationRepository _notificationRepository;
        public GetAllNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<Notification>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetAllNotifications(request.employeeCode);
        }
    }
}
