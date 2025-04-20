using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMS.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        readonly CMSDbContext _context;
        readonly JwtSettings _jwtSettings;
        public AuthRepository(CMSDbContext context,JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }
        public async Task<AuthResponseDto> Login(LoginDto request)
        {
            var hasher = new PasswordHasher<MasterEmployee>();
            var user = await _context.MasterEmployees.FirstOrDefaultAsync(e => e.Email == request.Email);
            if (user == null)
            {
                throw new UserNotFoundException("Invalid email or password.");
            }
            var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new UserNotFoundException("Invalid email or password.");
            }
            if (user.LastPasswordChanged.AddMonths(3) < DateTime.Now)
            {
                throw new PasswordRenewalException("Your password hasn't been renewed since last 3 months. Please renew it first and sign in.");
            }

            //var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken token = await GenerateToken(user);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.EmployeeCode,
                Role = user.Role,
                Name = user.EmployeeName,
                Email = user.Email,

            };
        }
        public async Task<JwtSecurityToken> GenerateToken(MasterEmployee user)
        {
            // Create Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.EmployeeName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("EID", user.ValueId.ToString())
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256); // Generally used this type
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }
    }
}
