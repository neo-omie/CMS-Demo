using MediatR;
using CMS.Domain.Entities;
using CMS.Application.DTOs;
using CMS.Application.Contracts.Persistence;

namespace CMS.Application.Features.Auth.Login
{
    public class LoginCommandHandler:IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IAuthRepository _authrepository;

        public LoginCommandHandler(IAuthRepository authrepository)
        {
            _authrepository = authrepository;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authrepository.Login(request.user);
        }
    }
}
