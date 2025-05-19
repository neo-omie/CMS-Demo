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
                },
                new ListOfCountries
                {
                    CountryId = 3,
                    Countries = "UK"
                },
                new ListOfCountries
                {
                    CountryId = 4,
                    Countries = "Russia"
                },
                new ListOfCountries
                {
                    CountryId = 5,
                    Countries = "China"
                },
                new ListOfCountries
                {
                    CountryId = 6,
                    Countries = "Japan"
                },
                new ListOfCountries
                {
                    CountryId = 7,
                    Countries = "Australia"
                },
                new ListOfCountries
                {
                    CountryId = 8,
                    Countries = "New Zealand"
                },
                new ListOfCountries
                {
                    CountryId = 9,
                    Countries = "Pakistan"
                },
                new ListOfCountries
                {
                    CountryId = 10,
                    Countries = "Germany"
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
                },
                new ListOfStates
                {
                    StateId = 3,
                    State = "Scotland",
                    CountryId = 3
                },
                new ListOfStates
                {
                    StateId = 4,
                    State = "Republic of Crimea",
                    CountryId = 4
                },
                new ListOfStates
                {
                    StateId = 5,
                    State = "Guangdong Province",
                    CountryId = 5
                },
                new ListOfStates
                {
                    StateId = 6,
                    State = "Hokkaido",
                    CountryId = 6
                },
                new ListOfStates
                {
                    StateId = 7,
                    State = "Queensland",
                    CountryId = 7
                },
                new ListOfStates
                {
                    StateId = 8,
                    State = "Auckland",
                    CountryId = 8
                },
                new ListOfStates
                {
                    StateId = 9,
                    State = "Sindh",
                    CountryId = 9
                },
                new ListOfStates
                {
                    StateId = 10,
                    State = "Berlin",
                    CountryId = 10
                },
                new ListOfStates
                {
                    StateId = 11,
                    State = "Delhi",
                    CountryId = 1
                },
                new ListOfStates
                {
                    StateId = 12,
                    State = "Texas",
                    CountryId = 2
                },
                new ListOfStates
                {
                    StateId = 13,
                    State = "Wales",
                    CountryId = 3
                },
                new ListOfStates
                {
                    StateId = 14,
                    State = "Republic of Tatarsan",
                    CountryId = 4
                },
                new ListOfStates
                {
                    StateId = 15,
                    State = "Qinghai",
                    CountryId = 5
                },
                new ListOfStates
                {
                    StateId = 16,
                    State = "Kanto",
                    CountryId = 6
                },
                new ListOfStates
                {
                    StateId = 17,
                    State = "New South Wales",
                    CountryId = 7
                },
                new ListOfStates
                {
                    StateId = 18,
                    State = "Wellington",
                    CountryId = 8
                },
                new ListOfStates
                {
                    StateId = 19,
                    State = "Balochistan",
                    CountryId = 9
                },
                new ListOfStates
                {
                    StateId = 20,
                    State = "Bavaria",
                    CountryId = 10
                },
                new ListOfStates
                {
                    StateId = 21,
                    State = "Tamil Nadu",
                    CountryId = 1
                },
                new ListOfStates
                {
                    StateId = 22,
                    State = "Florida",
                    CountryId = 2
                },
                new ListOfStates
                {
                    StateId = 23,
                    State = "Northern Ireland",
                    CountryId = 3
                },
                new ListOfStates
                {
                    StateId = 24,
                    State = "Altai Republic",
                    CountryId = 4
                },
                new ListOfStates
                {
                    StateId = 25,
                    State = "Tibet",
                    CountryId = 5
                },
                new ListOfStates
                {
                    StateId = 26,
                    State = "Kyushu-Okinawa",
                    CountryId = 6
                },
                new ListOfStates
                {
                    StateId = 27,
                    State = "Victoria",
                    CountryId = 7
                },
                new ListOfStates
                {
                    StateId = 28,
                    State = "Canterbury",
                    CountryId = 8
                },
                new ListOfStates
                {
                    StateId = 29,
                    State = "Gilgit Baltistan",
                    CountryId = 9
                },
                new ListOfStates
                {
                    StateId = 30,
                    State = "Saxony",
                    CountryId = 10
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
                },
                new ListofCity
                {
                    CityId = 3,
                    City = "Edinburgh",
                    StateId = 3
                },
                new ListofCity
                {
                    CityId = 4,
                    City = "Kerch",
                    StateId = 4
                },
                new ListofCity
                {
                    CityId = 5,
                    City = "Guangzhou",
                    StateId = 5
                },
                new ListofCity
                {
                    CityId = 6,
                    City = "Sapporo",
                    StateId = 6
                },
                new ListofCity
                {
                    CityId = 7,
                    City = "Brisbane",
                    StateId = 7
                },
                new ListofCity
                {
                    CityId = 8,
                    City = "Auckland City",
                    StateId = 8
                },
                new ListofCity
                {
                    CityId = 9,
                    City = "Karachi",
                    StateId = 9
                },
                new ListofCity
                {
                    CityId = 10,
                    City = "Bernau",
                    StateId = 10
                },
                new ListofCity
                {
                    CityId = 11,
                    City = "New Delhi",
                    StateId = 11
                },
                new ListofCity
                {
                    CityId = 12,
                    City = "Houston",
                    StateId = 12
                },
                new ListofCity
                {
                    CityId = 13,
                    City = "Cardiff",
                    StateId = 13
                },
                new ListofCity
                {
                    CityId = 14,
                    City = "Kazan",
                    StateId = 14
                },
                new ListofCity
                {
                    CityId = 15,
                    City = "Xining",
                    StateId = 15
                },
                new ListofCity
                {
                    CityId = 16,
                    City = "Tokyo",
                    StateId = 16
                },
                new ListofCity
                {
                    CityId = 17,
                    City = "Sydney",
                    StateId = 17
                },
                new ListofCity
                {
                    CityId = 18,
                    City = "Porirua",
                    StateId = 18
                },
                new ListofCity
                {
                    CityId = 19,
                    City = "Quetta",
                    StateId = 19
                },
                new ListofCity
                {
                    CityId = 20,
                    City = "Munich",
                    StateId = 20
                },
                new ListofCity
                {
                    CityId = 21,
                    City = "Chennai",
                    StateId = 21
                },
                new ListofCity
                {
                    CityId = 22,
                    City = "Miami",
                    StateId = 22
                },
                new ListofCity
                {
                    CityId = 23,
                    City = "Belfast",
                    StateId = 23
                },
                new ListofCity
                {
                    CityId = 24,
                    City = "Gorno-Altaysk",
                    StateId = 24
                },
                new ListofCity
                {
                    CityId = 25,
                    City = "Lhasa",
                    StateId = 25
                },
                new ListofCity
                {
                    CityId = 26,
                    City = "Miyazaki",
                    StateId = 26
                },
                new ListofCity
                {
                    CityId = 27,
                    City = "Melbourne",
                    StateId = 27
                },
                new ListofCity
                {
                    CityId = 28,
                    City = "Christchurch",
                    StateId = 28
                },
                new ListofCity
                {
                    CityId = 29,
                    City = "Gilgit",
                    StateId = 29
                },
                new ListofCity
                {
                    CityId = 30,
                    City = "Leipzig",
                    StateId = 30
                }
            );
        }
    }
}
