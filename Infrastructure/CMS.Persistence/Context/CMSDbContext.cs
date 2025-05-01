using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.EscalationMatrixMouMaster;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Application.Features.MasterEscalationMatrixContracts;
using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;

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
        public DbSet<GetEscalationMatrixContractDto> GetEscalationMatrixContractDtos { get; set; }
        public DbSet<MasterEscalationMatrixMou> MasterEscalationMatrixMous { get; set; }
        public DbSet<EscalationMatrixMoutDto> GetEscalationMatrixMouDtos { get; set; }
        public DbSet<MasterEmployee> MasterEmployees { get; set; }
        public DbSet<MasterDocument> MasterDocuments { get; set; }
        public DbSet<MasterCompany> MasterCompanies { get; set; }
        public DbSet<GetMastersDTO> GetCompanyDtos { get; set; }
        public DbSet<MasterApostille> MasterApostilles { get; set; }
        public DbSet<GetAllApostilleDto> GetApostillesDtos { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<GetAllDepartmentsDto> GetDepartmentsDtos { get; set; }
        public DbSet<ListOfCountries> Countries { get; set; }
        public DbSet<ListOfStates> States { get; set; }
        public DbSet<ListofCity> Cities { get; set; }
        public DbSet<ContractTypeMasters> contracts { get; set; }
        public DbSet<Contract> ContractsEntity { get; set; }
        public DbSet<GetAllContractsDto> GetContractsDtos { get; set; }
        public DbSet<GetContractByIdDto> GetContractByIdDtos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<GetAllDepartmentsDto>().Entity<GetAllDepartmentsDto>().HasNoKey();
            modelBuilder.Ignore<GetEscalationMatrixContractDto>().Entity<GetEscalationMatrixContractDto>().HasNoKey();
            modelBuilder.Ignore<EscalationMatrixMoutDto>().Entity<EscalationMatrixMoutDto>().HasNoKey();
            modelBuilder.Ignore<GetAllApostilleDto>().Entity<GetAllApostilleDto>().HasNoKey();
            modelBuilder.Ignore<GetAllContractsDto>().Entity<GetAllContractsDto>().HasNoKey();
            modelBuilder.Ignore<GetContractByIdDto>().Entity<GetContractByIdDto>().HasNoKey();

            modelBuilder.Entity<GetMastersDTO>().HasNoKey();
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.EmployeeCode);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.ValueId);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterEscalationMatrixMou>().HasAlternateKey(mamc => mamc.DepartmentId);

            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Department).WithMany().HasForeignKey(mamc => mamc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);


            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Department).WithMany().HasForeignKey(mamc => mamc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);

            //mastercompany location
            modelBuilder.Entity<ListOfStates>().HasOne(st => st.listofcountries).WithMany().HasForeignKey(st => st.CountryId);
            modelBuilder.Entity<ListofCity>().HasOne(ct => ct.listofStates).WithMany().HasForeignKey(ct=> ct.StateId);
            modelBuilder.Entity<MasterCompany>().HasOne(mc => mc.city).WithMany().HasForeignKey(mc => mc.CityId);
            //Company Cascading 


            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation1).WithMany().HasForeignKey(memc => memc.EscalationId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation2).WithMany().HasForeignKey(memc => memc.EscalationId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Escalation3).WithMany().HasForeignKey(memc => memc.EscalationId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasOne(memc => memc.Department).WithMany().HasForeignKey(memc => memc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);

            modelBuilder.Entity<MasterEscalationMatrixMou>().HasOne(memc => memc.Escalation1).WithMany().HasForeignKey(memc => memc.EscalationId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixMou>().HasOne(memc => memc.Escalation2).WithMany().HasForeignKey(memc => memc.EscalationId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixMou>().HasOne(memc => memc.Escalation3).WithMany().HasForeignKey(memc => memc.EscalationId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterEscalationMatrixMou>().HasOne(memc => memc.Department).WithMany().HasForeignKey(memc => memc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);


            modelBuilder.Entity<Contract>().HasOne(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId).HasPrincipalKey(d => d.DepartmentId);
            modelBuilder.Entity<Contract>().HasOne(c => c.ContractWithCompany).WithMany().HasForeignKey(c => c.ContractWithCompanyId).HasPrincipalKey(cc => cc.ValueId);
            modelBuilder.Entity<Contract>().HasOne(c => c.ContractType).WithMany().HasForeignKey(c => c.ContractTypeId).HasPrincipalKey(ct => ct.ValueId);
            modelBuilder.Entity<Contract>().HasOne(c => c.ApostilleType).WithMany().HasForeignKey(c => c.ApostilleTypeId).HasPrincipalKey(at => at.ValueId);
            modelBuilder.Entity<Contract>().HasOne(c => c.EmpCustodian).WithMany().HasForeignKey(c => c.EmpCustodianId).HasPrincipalKey(ec => ec.ValueId);
            
            modelBuilder.ApplyConfiguration(new MasterEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfigurations());

        }
    }
}
