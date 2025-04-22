using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CMS.Persistence.Context
{
    public class CMSDbContextFactory : IDesignTimeDbContextFactory<CMSDbContext>
    {
        public CMSDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connStr = configuration.GetConnectionString("CMSConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<CMSDbContext>();
            optionsBuilder.UseSqlServer(connStr);
            return new CMSDbContext(optionsBuilder.Options);
        }
    }
}
