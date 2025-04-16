using CMS.Application.DTOs;
using MediatR;

namespace CMS.Application.Features.Auth.Login
{
    public record LoginCommand(LoginDto user): IRequest<AuthResponseDto>;
}
