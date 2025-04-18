using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Identity;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CMS.Identity.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email or password.");
            }  

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                throw new UserNotFoundException("Invalid email or password.");
            }

            var lastPswdChanged = user.LastPasswordChanged.AddMonths(3);
            if (lastPswdChanged < DateTime.Now)
            {
                throw new PasswordRenewalException("Your password hasn't been renewed since last 3 months. Please renew it first and sign in.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken token = await _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Role = roles.FirstOrDefault(),
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
