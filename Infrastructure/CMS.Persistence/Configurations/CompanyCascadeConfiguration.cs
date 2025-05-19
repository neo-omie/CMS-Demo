using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Entities.CompanyMaster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Persistence.Configurations
{
    public class CompanyCascadeConfiguration : IEntityTypeConfiguration<ListOfCountries>, IEntityTypeConfiguration<ListOfStates>, IEntityTypeConfiguration<ListofCity>
    {
        public void Configure(EntityTypeBuilder<ListOfCountries> builder)
        {
            builder.HasData(
                new ListOfCountries
                {
                    CountryId = 1,
                    Countries = "India"
                },
                new ListOfCountries
                {
                    CountryId = 2,
                    Countries = "USA"
                }
            );
        }

        public void Configure(EntityTypeBuilder<ListOfStates> builder)
        {
            builder.HasData(
                new ListOfStates
                {
                    StateId = 1,
                    State = "Maharashtra",
                    CountryId = 1
                },
                new ListOfStates
                {
                    StateId = 2,
                    State = "California",
                    CountryId = 2
                }
            );
        }

        public void Configure(EntityTypeBuilder<ListofCity> builder)
        {
            builder.HasData(
                new ListofCity
                {
                    CityId = 1,
                    City = "Mumbai",
                    StateId = 1
                },
                new ListofCity
                {
                    CityId = 2,
                    City = "Los Angeles",
                    StateId = 2
                }
            );
        }
    }
}
