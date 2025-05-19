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
    public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractTypeMasters>
    {
        public void Configure(EntityTypeBuilder<ContractTypeMasters> builder)
        {
            builder.HasData(
                new ContractTypeMasters
                {
                    ValueId = 1,
                    ContractTypeName = "Service",
                    Status = true,
                    IsDeleted = false
                },
                new ContractTypeMasters
                {
                    ValueId = 2,
                    ContractTypeName = "AMC",
                    Status = true,
                    IsDeleted = false
                },
                new ContractTypeMasters
                {
                    ValueId = 3,
                    ContractTypeName = "NDA",
                    Status = false,
                    IsDeleted = false
                },
                new ContractTypeMasters
                {
                    ValueId = 4,
                    ContractTypeName = "CSR",
                    Status = false,
                    IsDeleted = false
                },
                new ContractTypeMasters
                {
                    ValueId = 5,
                    ContractTypeName = "HR",
                    Status = true,
                    IsDeleted = true
                }
            );
        }
    }
}
