using CMS.Domain.Entities;
using CMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Context
{
    public class CMSDbContext:DbContext
    {
        public CMSDbContext(DbContextOptions<CMSDbContext> options) : base(options)
        {
        }
        public DbSet<MasterApprovalMatrixContract> MasterApprovalMatrixContracts { get; set; }
        public DbSet<MasterEmployee> MasterEmployees { get; set; }
        public DbSet<MasterDocument> MasterDocuments { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.EmployeeCode);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.ValueId);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.ApplyConfiguration(new MasterEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        }
    }
}
