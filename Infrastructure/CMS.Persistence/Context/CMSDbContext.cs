using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
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
        public DbSet<MasterApprovalMatrixMOU> MasterApprovalMatrixMOUs { get; set; }

        public DbSet<MasterEscalationMatrixContract> MasterEscalationMatrixContracts { get; set; }
        public DbSet<MasterEmployee> MasterEmployees { get; set; }
        public DbSet<MasterDocument> MasterDocuments { get; set; }
        public DbSet<MasterCompany> MasterCompanies { get; set; }
        public DbSet<MasterApostille> MasterApostilles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ListOfCountries> Countries { get; set; }
        public DbSet<ListOfStates> States { get; set; }
        public DbSet<ListofCity> Cities { get; set; }
        public DbSet<ContractTypeMasters> contracts { get; set; }



      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.EmployeeCode);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.ValueId);

            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);


            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation1).WithMany().HasForeignKey(memc => memc.EscalationId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation2).WithMany().HasForeignKey(memc => memc.EscalationId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation3).WithMany().HasForeignKey(memc => memc.EscalationId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.ApplyConfiguration(new MasterEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfigurations());

        }
    }
}
