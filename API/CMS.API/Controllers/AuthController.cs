using CMS.Application.DTOs;
using CMS.Application.Features.Auth.Login;
using CMS.Application.Features.Auth.RefreshPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        readonly IMediator _mediatR;
        public AuthController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var resp = await _mediatR.Send(new LoginCommand(loginDto));
            return Ok(resp);
        }

        [HttpPost("refreshPassword")]
        public async Task<ActionResult<string>> RefreshPassword(RefreshPasswordDto refreshPassword)
        {
            var resp = await _mediatR.Send(new RefreshPasswordCommand(refreshPassword));
            return Ok(resp);
        }
       
    }
}
