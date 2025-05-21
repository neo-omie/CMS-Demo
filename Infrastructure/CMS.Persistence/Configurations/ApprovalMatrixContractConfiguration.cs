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
    public class ApprovalMatrixContractConfiguration : IEntityTypeConfiguration<MasterApprovalMatrixContract>
    {
        public void Configure(EntityTypeBuilder<MasterApprovalMatrixContract> builder)
        {
            builder.HasData(
                new MasterApprovalMatrixContract
                {
                    MasterApprovalMatrixContractId = 1,
                    DepartmentId = 1,
                    ApproverId1 = "NEO1",
                    ApproverId2 = "NEO1",
                    ApproverId3 = "NEO1",
                    NumberOfDays = 5,
                    CreatedBy = "NEO1",
                    CreatedOn = DateTime.Now,
                    UpdatedBy = "NEO1",
                    UpdateOn = DateTime.Now
                },
                new MasterApprovalMatrixContract
                {
                    MasterApprovalMatrixContractId = 2,
                    DepartmentId = 2,
                    ApproverId1 = "NEO2",
                    ApproverId2 = "NEO6",
                    ApproverId3 = "NEO2",
                    NumberOfDays = 10,
                    CreatedBy = "NEO1",
                    CreatedOn = DateTime.Now,
                    UpdatedBy = "NEO1",
                    UpdateOn = DateTime.Now
                },
                new MasterApprovalMatrixContract
                {
                    MasterApprovalMatrixContractId = 3,
                    DepartmentId = 3,
                    ApproverId1 = "NEO3",
                    ApproverId2 = "NEO3",
                    ApproverId3 = "NEO3",
                    NumberOfDays = 7,
                    CreatedBy = "NEO1",
                    CreatedOn = DateTime.Now,
                    UpdatedBy = "NEO1",
                    UpdateOn = DateTime.Now
                }
            );
        }
    }
}
