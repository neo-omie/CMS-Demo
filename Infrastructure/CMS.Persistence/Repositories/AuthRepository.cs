using Azure.Core;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
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
                throw new UserNotFoundException("User does not exists. Please check if the email entered is correct or not.");
            }
            var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new UserNotFoundException("Invalid email or password. Please try again.");
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

        public async Task<string> RefreshPasswordAsync(RefreshPasswordDto refreshPassword)
        {
            var hasher = new PasswordHasher<MasterEmployee>();
            var user = await _context.MasterEmployees.FirstOrDefaultAsync(e => e.Email == refreshPassword.Email);
            if (user == null)
            {
                throw new UserNotFoundException("User does not exists. Please check if the email entered is correct or not.");
            }
            var checkPassword = hasher.VerifyHashedPassword(user, user.Password, refreshPassword.OldPassword);
            if (checkPassword != PasswordVerificationResult.Success)
            {
                throw new UserNotFoundException("Invalid email or password. Please try again.");
            }
            if (user.LastPasswordChanged.AddMonths(3) < DateTime.Now)
            {
                if(refreshPassword.OldPassword == refreshPassword.NewPassword)
                {
                    throw new Exception("Your old password can't be same as the new password. Please create a new one.");
                }
                user.Password = hasher.HashPassword(null, refreshPassword.NewPassword);
                user.LastPasswordChanged = DateTime.Now;
                _context.MasterEmployees.Update(user);
                await _context.SaveChangesAsync();
                string result = "Your password has been updated. You can now log in with your new password!";
                return result;
            }
            return "Some error occurred. Please enter correct credentials.";
        }
    }
}
