using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using MediatR;

namespace CMS.Application.Features.Notifications.Commands.DeleteCommand
{
    public class DeleteCommandHandler : IRequestHandler<DeleteNotification, bool>
    {
        private INotificationRepository _notificationRepository;

        public DeleteCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        

        Task<bool> IRequestHandler<DeleteNotification, bool>.Handle(DeleteNotification request, CancellationToken cancellationToken)
        {
         return   _notificationRepository.DeleteNotification(request.id);
        }
    }
}
