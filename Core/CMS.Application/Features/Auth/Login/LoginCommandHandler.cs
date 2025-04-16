using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using CMS.Application.Contracts.Identity;

namespace CMS.Application.Features.Auth.Login
{
    public class LoginCommandHandler:IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IAuthService _authservice;

        public LoginCommandHandler(IAuthService authservice)
        {
            _authservice = authservice;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authservice.Login(request.Auth);
        }
    }
}
