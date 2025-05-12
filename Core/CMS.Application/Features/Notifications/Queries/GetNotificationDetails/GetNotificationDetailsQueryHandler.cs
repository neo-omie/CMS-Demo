using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Notifications.Queries.GetNotificationDetails
{
    public class GetNotificationDetailsQueryHandler : IRequestHandler<GetNotificationDetailsQuery, Notification>
    {
        readonly INotificationRepository _notificationRepository;
        public GetNotificationDetailsQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Notification> Handle(GetNotificationDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetNotificationDetails(request.id, request.employeeCode);
        }
    }
}
