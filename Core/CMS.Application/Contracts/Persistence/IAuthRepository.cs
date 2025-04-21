using CMS.Application.DTOs;

namespace CMS.Application.Contracts.Persistence
{
    public interface IAuthRepository
    {
        public Task<AuthResponseDto> Login(LoginDto loginDto);
        public Task<string> RefreshPasswordAsync(RefreshPasswordDto refreshPassword);
    }
}
