
using CMS.Application.Contracts.Identity;
using CMS.Application.DTOs;
using CMS.Application.Features.Auth.Login;
using CMS.Application.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            var response = await _mediatR.Send(new LoginCommand(loginDto));
            return Ok(response);
        }
       
    }
}
