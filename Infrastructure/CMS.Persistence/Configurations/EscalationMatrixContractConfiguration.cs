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
    public class EscalationMatrixContractConfiguration : IEntityTypeConfiguration<MasterEscalationMatrixContract>
    {
        public void Configure(EntityTypeBuilder<MasterEscalationMatrixContract> builder)
        {
            builder.HasData(

                new MasterEscalationMatrixContract
                {
                    MatrixContractId = 1,
                    EscalationId1 = "NEO1",
                    EscalationId2 = "NEO1",
                    EscalationId3 = "NEO1",
                    TriggerDaysEscalation1 = 3,
                    TriggerDaysEscalation2 = 6,
                    TriggerDaysEscalation3 = 9,
                    DepartmentId = 1
                },
                new MasterEscalationMatrixContract
                {
                    MatrixContractId = 2,
                    EscalationId1 = "NEO2",
                    EscalationId2 = "NEO2",
                    EscalationId3 = "NEO2",
                    TriggerDaysEscalation1 = 4,
                    TriggerDaysEscalation2 = 6,
                    TriggerDaysEscalation3 = 8,
                    DepartmentId = 2
                },
                new MasterEscalationMatrixContract
                {
                    MatrixContractId = 3,
                    EscalationId1 = "NEO3",
                    EscalationId2 = "NEO3",
                    EscalationId3 = "NEO3",
                    TriggerDaysEscalation1 = 2,
                    TriggerDaysEscalation2 = 3,
                    TriggerDaysEscalation3 = 5,
                    DepartmentId = 3
                },
                new MasterEscalationMatrixContract
                {
                    MatrixContractId = 4,
                    EscalationId1 = "NEO4",
                    EscalationId2 = "NEO4",
                    EscalationId3 = "NEO4",
                    TriggerDaysEscalation1 = 1,
                    TriggerDaysEscalation2 = 2,
                    TriggerDaysEscalation3 = 3,
                    DepartmentId = 4
                }
            );
        }
    }
}
