using CMS.Application.Features.Notifications.Queries.GetAllNotifications;
using CMS.Application.Features.Notifications.Queries.GetNotificationDetails;
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

        [Route("{employeeCode}")]
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications([FromRoute] string employeeCode)
        {
            var allNotifs = await _mediator.Send(new GetAllNotificationsQuery(employeeCode));
            return Ok(allNotifs);
        }
        [Route("{id}/{employeeCode}")]
        [HttpGet]
        public async Task<IActionResult> GetNotificationDetails([FromRoute] int id, [FromRoute] string employeeCode)
        {
            var notifs = await _mediator.Send(new GetNotificationDetailsQuery(id, employeeCode));
            return Ok(notifs);
        }

    }
}
