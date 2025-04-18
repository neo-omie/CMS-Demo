using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CMS.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "41116008-6086-1aaa-b923-2879a6680b9a", // Admin
                    UserId = "41776062-1111-1aba-a111-2879a6680b9a" // Admin User
                },
                new IdentityUserRole<string>
                {
                    RoleId = "41116008-6086-1aab-b923-2879a6680b9a", // Management User
                    UserId = "41776062-1111-1abb-a111-2879a6680b9a" // Sarthak
                }
            );
        }
    }
}
