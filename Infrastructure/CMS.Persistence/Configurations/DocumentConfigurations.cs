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
    public class DocumentConfigurations : IEntityTypeConfiguration<MasterDocument>
    {
        public void Configure(EntityTypeBuilder<MasterDocument> builder)
        {
            builder.HasData(
                new MasterDocument
                {
                    ValueId = 1,
                    DocumentName = "Doc 1",
                    IsDeleted = false,
                    status = Domain.Constants.Status.Active,
                },
                new MasterDocument
                {
                    ValueId = 2,
                    DocumentName = "Doc 2",
                    IsDeleted = false,
                    status = Domain.Constants.Status.Active,
                },
                new MasterDocument
                {
                    ValueId = 3,
                    DocumentName = "Doc 3",
                    IsDeleted = false,
                    status = Domain.Constants.Status.Active,
                }
                );
        }
    }
}
