using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CMS.Domain.Entities;
//using CMS.Identity.Models.Constants;

namespace CMS.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<MasterEmployee>
    {
        public void Configure(EntityTypeBuilder<MasterEmployee> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new MasterEmployee
                {
                    Id = "41776062-1111-1aba-a111-2879a6680b9a",
                    ValueId = 1,
                    EmployeeName = "Admin",
                    Email = "admin@cms.com",
                    NormalizedEmail = "ADMIN@CMS.COM",
                    IsDeleted = false,
                    EmployeeCode = "NEO1",
                    Unit = "Dadar",
                    EmployeeMobile = 7777766666,
                    EmployeeExtension = "Main person",
                    DepartmentId = 100,
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                },
                new MasterEmployee
                {
                    Id = "41776062-1111-1abb-a111-2879a6680b9a",
                    ValueId = 2,
                    EmployeeName = "Sarthak Lembhe",
                    Email = "sarthak@neosoft.com",
                    NormalizedEmail = "SARTHAK@NEOSOFT.COM",
                    IsDeleted = false,
                    EmployeeCode = "NEO2",
                    Unit = "Dadar",
                    EmployeeMobile = 9999988888,
                    EmployeeExtension = "IT Smart",
                    DepartmentId = 101,
                    PasswordHash = hasher.HashPassword(null, "Sarthak@12"),
                    LastPasswordChanged = new DateTime(2025, 01, 10)
                }
            );
        }
    }
}
