using MediatR;
using CMS.Application.Contracts.Identity;
using CMS.Application.DTOs;

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
            var userSignin = await _authservice.Login(request.user);
            return userSignin;
        }
    }
}
