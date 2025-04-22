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
                    DepartmentId = 1,
                    DepartmentName = "Admin Support",
                },
                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "IT",
                },
                new Department
                {
                    DepartmentId = 3,
                    DepartmentName = "HR",
                },
                new Department
                {
                    DepartmentId = 4,
                    DepartmentName = "Finance",
                },
                new Department
                {
                    DepartmentId = 5,
                    DepartmentName = "Maintenance",
                }
            );

        }
    }
}
