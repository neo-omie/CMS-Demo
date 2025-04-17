using System;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CMSDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("CMSConnectionString")));
            return services;
        }
    }
}
