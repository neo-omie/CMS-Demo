using CMS.Application.Features.Notifications.Commands.DeleteCommand;
using CMS.Application.Features.Notifications.Queries.GetAllNotifications;
using CMS.Application.Features.Notifications.Queries.GetNotificationDetails;
using CMS.Application.Features.Notifications.Queries.UnreadNotificationsCount;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        readonly IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Route("{employeeCode}")]
        [HttpGet("{pageNumber}/{pageSize}/{employeeCode}")]
        public async Task<IActionResult> GetAllNotifications([FromRoute] string employeeCode, [FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            var allNotifs = await _mediator.Send(new GetAllNotificationsQuery(pageNumber, pageSize, employeeCode));
            return Ok(allNotifs);
        }
        [Route("{id}/{employeeCode}")]
        [HttpGet]
        public async Task<IActionResult> GetNotificationDetails([FromRoute] int id, [FromRoute] string employeeCode)
        {
            var notifs = await _mediator.Send(new GetNotificationDetailsQuery(id, employeeCode));
            return Ok(notifs);
        }
        [HttpGet("UnreadNotifications/{employeeCode}")]
        public async Task<IActionResult> UnreadNotificationsCount([FromRoute] string employeeCode)
        {
            var unreadNotifsCount = await _mediator.Send(new UnreadNotificationsCountQuery(employeeCode));
            return Ok(unreadNotifsCount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> GetNotification([FromRoute] int id)
        {
            var notif = await _mediator.Send(new DeleteNotification(id));
            return Ok(notif);

        }

    }
}
