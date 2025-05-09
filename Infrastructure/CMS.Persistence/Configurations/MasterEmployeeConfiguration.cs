﻿using CMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Persistence.Configurations
{
    public class MasterEmployeeConfiguration : IEntityTypeConfiguration<MasterEmployee>
    {
        public void Configure(EntityTypeBuilder<MasterEmployee> builder)
        {
            var hasher = new PasswordHasher<MasterEmployee>();
            builder.HasData(
                new MasterEmployee
                {
                    ValueId = 1,
                    EmployeeName = "Admin",
                    Email = "admin@cms.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO1",
                    Unit = "Thane",
                    Role = "Admin",
                    EmployeeMobile = 7777766666,
                    EmployeeExtension = 2467,
                    DepartmentId = 1,
                    Password = hasher.HashPassword(null, "Admin@123"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                },
                new MasterEmployee
                {
                    ValueId = 2,
                    EmployeeName = "Sarthak Lembhe",
                    Email = "sarthak@neosoft.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO2",
                    Unit = "Thane",
                    Role = "MOU_Approver",
                    EmployeeMobile = 9999988888,
                    EmployeeExtension = 8976,
                    DepartmentId = 2,
                    Password = hasher.HashPassword(null, "Sarthak@12"),
                    LastPasswordChanged = new DateTime(2025, 01, 10)
                }         
            );
        }
    }
}
