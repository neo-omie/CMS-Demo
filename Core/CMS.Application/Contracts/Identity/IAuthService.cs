using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Models.Identity;

namespace CMS.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginDTO loginDto);
    }
}
