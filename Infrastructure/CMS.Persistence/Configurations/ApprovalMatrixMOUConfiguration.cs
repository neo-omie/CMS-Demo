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
    public class ApprovalMatrixMOUConfiguration : IEntityTypeConfiguration<MasterApprovalMatrixMOU>
    {
        public void Configure(EntityTypeBuilder<MasterApprovalMatrixMOU> builder)
        {
            builder.HasData(
                new MasterApprovalMatrixMOU
                {
                    MasterApprovalMatrixMOUId = 1,
                    DepartmentId = 1,
                    ApproverId1 = "NEO1",
                    ApproverId2 = "NEO1",
                    ApproverId3 = "NEO1",
                    NumberOfDays = 10
                },
                new MasterApprovalMatrixMOU
                {
                    MasterApprovalMatrixMOUId = 2,
                    DepartmentId = 2,
                    ApproverId1 = "NEO6",
                    ApproverId2 = "NEO2",
                    ApproverId3 = "NEO2",
                    NumberOfDays = 10
                },
                new MasterApprovalMatrixMOU
                {
                    MasterApprovalMatrixMOUId = 3,
                    DepartmentId = 3,
                    ApproverId1 = "NEO3",
                    ApproverId2 = "NEO3",
                    ApproverId3 = "NEO3",
                    NumberOfDays = 7
                },
                new MasterApprovalMatrixMOU
                {
                    MasterApprovalMatrixMOUId = 4,
                    DepartmentId = 4,
                    ApproverId1 = "NEO4",
                    ApproverId2 = "NEO4",
                    ApproverId3 = "NEO4",
                    NumberOfDays = 10
                },
                new MasterApprovalMatrixMOU
                {
                    MasterApprovalMatrixMOUId = 5,
                    DepartmentId = 5,
                    ApproverId1 = "NEO5",
                    ApproverId2 = "NEO5",
                    ApproverId3 = "NEO5",
                    NumberOfDays = 8
                }
            );
        }
    }
}
