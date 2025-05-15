using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CMS.Application.Features.Notifications.Queries.UnreadNotificationsCount
{
    public record UnreadNotificationsCountQuery(string employeeCode) : IRequest<int>;
}
