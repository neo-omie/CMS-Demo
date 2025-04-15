using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Identity.Context;
using CMS.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CMSIdentityDbContext>(options =>

                   options.UseSqlServer(configuration.GetConnectionString("CMSConnectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<CMSIdentityDbContext>().AddDefaultTokenProviders();

            return services;
        }
    }
}
