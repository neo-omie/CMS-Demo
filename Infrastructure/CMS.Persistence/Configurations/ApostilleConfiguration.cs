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
    public class ApostilleConfiguration : IEntityTypeConfiguration<MasterApostille>
    {
        public void Configure(EntityTypeBuilder<MasterApostille> builder)
        {
            builder.HasData(
                new MasterApostille
                {
                    ValueId = 1,
                    ApostilleName = "Stamp Paper",
                    Status = false,
                    IsDeleted = false
                },
                new MasterApostille
                {
                    ValueId = 2,
                    ApostilleName = "Frankin",
                    Status = false,
                    IsDeleted = false
                },
                new MasterApostille
                {
                    ValueId = 3,
                    ApostilleName = "Notary",
                    Status = true,
                    IsDeleted = false
                },
                new MasterApostille
                {
                    ValueId = 4,
                    ApostilleName = "Affidavit",
                    Status = false,
                    IsDeleted = false
                }
            );
        }
    }
}
