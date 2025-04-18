using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CMS.Identity.Context
{
    public class CMSIdentityDbContextFactory : IDesignTimeDbContextFactory<CMSIdentityDbContext>
    {
        public CMSIdentityDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connStr = configuration.GetConnectionString("CMSConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<CMSIdentityDbContext>();
            optionsBuilder.UseSqlServer(connStr);
            return new CMSIdentityDbContext(optionsBuilder.Options);
        }
    }
}
