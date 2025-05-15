using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.Notifications.Queries.UnreadNotificationsCount
{
    public class UnreadNotificationsCountQueryHandler : IRequestHandler<UnreadNotificationsCountQuery, int>
    {
        readonly INotificationRepository _notificationRepository;
        public UnreadNotificationsCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<int> Handle(UnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.UnreadNotificationsCount(request.employeeCode);
        }
    }
}
