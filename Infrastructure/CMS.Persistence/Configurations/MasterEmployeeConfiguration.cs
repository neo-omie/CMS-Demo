using CMS.Domain.Entities;
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
                    Email = "omigaming3123@gmail.com",
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
                    Email = "sarthak.lembhe@neosoftmail.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO2",
                    Unit = "Thane",
                    Role = "Contract_Approver",
                    EmployeeMobile = 9999988888,
                    EmployeeExtension = 8976,
                    DepartmentId = 2,
                    Password = hasher.HashPassword(null, "Sarthak@12"),
                    LastPasswordChanged = new DateTime(2025, 01, 10)
                },
                new MasterEmployee
                {
                    ValueId = 3,
                    EmployeeName = "Sakthish Nadar",
                    Email = "sakthish.nadar@neosoftmail.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO3",
                    Unit = "Pune",
                    Role = "Contract_Approver",
                    EmployeeMobile = 8888899999,
                    EmployeeExtension = 6969,
                    DepartmentId = 3,
                    Password = hasher.HashPassword(null, "Sakt@12"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                },
                new MasterEmployee
                {
                    ValueId = 4,
                    EmployeeName = "Shreekant Panigrahi",
                    Email = "shreekant.panigrahi@neosoftmail.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO4",
                    Unit = "Pune",
                    Role = "Contract_Approver",
                    EmployeeMobile = 7777788888,
                    EmployeeExtension = 1111,
                    DepartmentId = 4,
                    Password = hasher.HashPassword(null, "Shreek@12"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                },
                new MasterEmployee
                {
                    ValueId = 5,
                    EmployeeName = "Govind Lohar",
                    Email = "govind.lohar@neosoftmail.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO5",
                    Unit = "Indore",
                    Role = "Contract_Approver",
                    EmployeeMobile = 7676587876,
                    EmployeeExtension = 4321,
                    DepartmentId = 5,
                    Password = hasher.HashPassword(null, "Govind@12"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                },
                new MasterEmployee
                {
                    ValueId = 6,
                    EmployeeName = "Om Auti",
                    Email = "om.auti@neosoftmail.com",
                    IsDeleted = false,
                    EmployeeCode = "NEO6",
                    Unit = "Indore",
                    Role = "Contract_Approver",
                    EmployeeMobile = 9876543210,
                    EmployeeExtension = 1234,
                    DepartmentId = 2,
                    Password = hasher.HashPassword(null, "Omie@12"),
                    LastPasswordChanged = new DateTime(2025, 04, 15)
                }
            );
        }
    }
}
