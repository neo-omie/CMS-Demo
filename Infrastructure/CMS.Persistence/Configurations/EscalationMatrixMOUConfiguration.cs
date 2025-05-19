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
    public class EscalationMatrixMOUConfiguration : IEntityTypeConfiguration<MasterEscalationMatrixMou>
    {
        public void Configure(EntityTypeBuilder<MasterEscalationMatrixMou> builder)
        {
            builder.HasData(
                new MasterEscalationMatrixMou
                {
                    MatrixMouId = 1,
                    EscalationId1 = "NEO1",
                    EscalationId2 = "NEO1",
                    EscalationId3 = "NEO1",
                    TriggerDaysEscalation1 = 2,
                    TriggerDaysEscalation2 = 3,
                    TriggerDaysEscalation3 = 5,
                    DepartmentId = 1
                },
                new MasterEscalationMatrixMou
                {
                    MatrixMouId = 2,
                    EscalationId1 = "NEO2",
                    EscalationId2 = "NEO2",
                    EscalationId3 = "NEO2",
                    TriggerDaysEscalation1 = 2,
                    TriggerDaysEscalation2 = 4,
                    TriggerDaysEscalation3 = 8,
                    DepartmentId = 2
                },
                new MasterEscalationMatrixMou
                {
                    MatrixMouId = 3,
                    EscalationId1 = "NEO3",
                    EscalationId2 = "NEO3",
                    EscalationId3 = "NEO3",
                    TriggerDaysEscalation1 = 3,
                    TriggerDaysEscalation2 = 5,
                    TriggerDaysEscalation3 = 8,
                    DepartmentId = 3
                },
                new MasterEscalationMatrixMou
                {
                    MatrixMouId = 4,
                    EscalationId1 = "NEO4",
                    EscalationId2 = "NEO4",
                    EscalationId3 = "NEO4",
                    TriggerDaysEscalation1 = 2,
                    TriggerDaysEscalation2 = 3,
                    TriggerDaysEscalation3 = 6,
                    DepartmentId = 4
                }
            );
        }
    }
}
