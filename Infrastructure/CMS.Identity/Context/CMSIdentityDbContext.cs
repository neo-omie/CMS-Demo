using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using CMS.Identity.Configurations;
using CMS.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.Identity.Context
{
    public class CMSIdentityDbContext: IdentityDbContext<MasterEmployee>
    {
        public CMSIdentityDbContext(DbContextOptions<CMSIdentityDbContext> options) :base(options)
        {           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MasterEmployee>().HasIndex(u => u.EmployeeCode).IsUnique();
            builder.Entity<MasterEmployee>().HasIndex(u => u.ValueId).IsUnique();
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());

        }
    }
}
