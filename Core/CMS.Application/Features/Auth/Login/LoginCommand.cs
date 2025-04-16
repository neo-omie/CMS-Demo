

namespace CMS.Application.Features.Auth.Login
{
    public class LoginCommand: IRequest<AuthResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
