using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department
                {
                    DepartmentId = 100,
                    DepartmentName = "Admin Support",
                },
                new Department
                {
                    DepartmentId = 101,
                    DepartmentName = "IT",
                },
                new Department
                {
                    DepartmentId = 102,
                    DepartmentName = "HR",
                },
                new Department
                {
                    DepartmentId = 103,
                    DepartmentName = "Finance",
                },
                new Department
                {
                    DepartmentId = 104,
                    DepartmentName = "Maintenance",
                }
            );

        }
    }
}
