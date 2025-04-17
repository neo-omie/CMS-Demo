using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
//using CMS.Identity.Models.Constants;

namespace CMS.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new ApplicationUser
                {
                    Id = "41776062 - 1111 - 1aba - a111 - 2879a6680b9a",
                    Name = "Admin",
                    Email = "admin@cms.com",
                    NormalizedEmail = "ADMIN@CMS.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin@123")
                },
                new ApplicationUser
                {
                    Id = "41776062 - 1111 - 1abb- a111 - 2879a6680b9a",
                    Name = "Sarthak",
                    Email = "sarthak@neosoft.com",
                    NormalizedEmail = "SARTHAK@NEOSOFT.COM",
                    PasswordHash = hasher.HashPassword(null, "Sarthak@12"),
                    //Department = Departments.Admin
                }
            );
        }
    }
}
