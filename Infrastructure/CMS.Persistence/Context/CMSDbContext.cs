using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Context
{
    public class CMSDbContext:DbContext
    {
        public CMSDbContext(DbContextOptions<CMSDbContext> options) : base(options)
        {
        }

        public DbSet<MasterEmployee> MasterEmployees { get; set; }
        public DbSet<MasterDocument> MasterDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
