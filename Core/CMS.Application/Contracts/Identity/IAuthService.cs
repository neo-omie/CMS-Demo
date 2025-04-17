using CMS.Application.DTOs;

namespace CMS.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
