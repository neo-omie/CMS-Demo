using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "41116008-6086-1aaa-b923-2879a6680b9a",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"

                },
                new IdentityRole
                {
                    Id = "41116008-6086-1aab-b923-2879a6680b9a",
                    Name = "Management User",
                    NormalizedName = "MANAGEMENT USER"
                },
                new IdentityRole
                {
                    Id = "41116008-6086-1bba-b923-2879a6680b9a",
                    Name = "MOU User",
                    NormalizedName = "MOU USER"
                },
                new IdentityRole
                {
                    Id = "41116008-6086-1bbb-b923-2879a6680b9a",
                    Name = "MOU Approver",
                    NormalizedName = "MOU APPROVER"
                },
                new IdentityRole
                {
                    Id = "41116008-6086-1cca-b923-2879a6680b9a",
                    Name = "Contract User",
                    NormalizedName = "CONTRACT USER"
                },
                new IdentityRole
                {
                    Id = "41116008-6086-1ccb-b923-2879a6680b9a",
                    Name = "Contract Approver",
                    NormalizedName = "CONTRACT APPROVER"
                }
            );
        }
    }
}
