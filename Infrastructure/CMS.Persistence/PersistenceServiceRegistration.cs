using System.Text;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using CMS.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CMS.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CMSDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("CMSConnectionString")));
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IMasterEmployeeRepository, MasterEmployeeRepository>();
            services.AddScoped<IMasterCompanyRepository, MasterCompanyRepository>();
            services.AddScoped<IMasterApprovalMatrixContractRepository, MasterApprovalMatrixContractRepository>();
            services.AddScoped<IMasterEscalationMatrixContractRepository, MasterEscalationMatrixContractRepository>();
            services.AddScoped<IMasterApprovalMatrixMOURepository, MasterApprovalMatrixMOURepository>();
            services.AddScoped<IMasterApostilleRepository, MasterApostilleRepository>();
            services.AddScoped<IContractTypeMasterRepository, ContractTypeMasterRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<ICompanyCascadeRepository, CompanyCascadeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };
            });
            return services;
        }
    }
}
