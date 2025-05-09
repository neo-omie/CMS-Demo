using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.Notifications.Queries.GetNotificationDetails
{
    public record GetNotificationDetailsQuery(int id, string employeeCode) : IRequest<Notification>;
}
