using CMS.Application.Features.MasterCompanies;
using CMS.Application.Features.Departments.Queries.GetAllDepartments;
using CMS.Application.Features.EscalationMatrixMouMaster;
using CMS.Application.Features.MasterApostilles.ApostilleDtos;
using CMS.Application.Features.MasterEscalationMatrixContracts;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetAllApprovalMatrixContract;
using CMS.Application.Features.ApprovalMatrixContract.Queries.GetApprovalMatrixContractById;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOU;
using CMS.Application.Features.ApprovalMatrixMOU.Queries.GetAllApprovalMatrixMOUById;
using CMS.Domain.Entities;
using CMS.Domain.Entities.CompanyMaster;
using CMS.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Application.Features.ClassifiedContracts.Queries.GetAllClassifiedContracts;
using CMS.Application.Features.ClassifiedContracts.Queries.GetClassifiedContractById;
using CMS.Application.Features.PostTermination.Command.AddCommand;
using CMS.Application.Features.AddendumContracts.AddendumContractDto;
using CMS.Application.Features.AuditTrails.Queries.GetAllAudits;
using CMS.Application.Features.Notifications.Queries.GetAllNotifications;

namespace CMS.Persistence.Context
{
    public class CMSDbContext:DbContext
    {
        public CMSDbContext(DbContextOptions<CMSDbContext> options) : base(options)
        {
        }
        public DbSet<GetAllApprovalMatrixContractDTO> GetAllApprovalMatrixContractDTOs {  get; set; }
        public DbSet<GetApprovalMatrixContractByIdDto> GetApprovalMatrixContractByIdDtos { get; set; }
        public DbSet<GetAllApprovalMatrixMOUDto> GetAllApprovalMatrixMOUDtos { get; set; }
        public DbSet<GetAllApprovalMatrixMOUByIdDto> GetAllApprovalMatrixMOUByIdDtos { get; set; }
        public DbSet<MasterApprovalMatrixContract> MasterApprovalMatrixContracts { get; set; }
        public DbSet<MasterApprovalMatrixMOU> MasterApprovalMatrixMOUs { get; set; }
        //Addition of AddendumContract.
        public DbSet<AddendumContract> AddendumContracts { get; set; }
        public DbSet<GetAddendumContractByIdDto> GetAddendumContractByIdDtos { get; set; }
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

        public DbSet<ClassifiedContract> ClassifiedContracts { get; set; }
        public DbSet<GetAllClassifiedContractsDto> GetClassifiedContractsDtos { get; set; }
        public DbSet<GetClassifiedContractByIdDto> GetClassifiedContractByIdDtos { get; set; }


        public DbSet<Notification> ContractNotifications { get; set; }
        public DbSet<GetAllNotificationsDto> ContractNotificationsDto { get; set; }

        public DbSet<PostTerminationNotice> PostTerminationNotices { get; set; }
        public DbSet<ClassifiedPostTerminationNotice> ClassifiedPostTerminationNotices { get; set; }
        public DbSet<NoticeWithdrawal> NoticeWithdrawals { get; set; }
        public DbSet<ClassifiedNoticeWithdrawal> ClassifiedNoticeWithdrawals { get; set; }

        public DbSet<AuditTrail> AuditTrails { get; set; }

        public DbSet<GetAllAuditDto> GetAllAudits { get; set; }
        public DbSet<ContractsCount> ContractsCounter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<GetAllDepartmentsDto>().Entity<GetAllDepartmentsDto>().HasNoKey();
            modelBuilder.Ignore<GetEscalationMatrixContractDto>().Entity<GetEscalationMatrixContractDto>().HasNoKey();
            modelBuilder.Ignore<EscalationMatrixMoutDto>().Entity<EscalationMatrixMoutDto>().HasNoKey();
            modelBuilder.Ignore<GetAllApostilleDto>().Entity<GetAllApostilleDto>().HasNoKey();
            modelBuilder.Ignore<GetAllContractsDto>().Entity<GetAllContractsDto>().HasNoKey();
            modelBuilder.Ignore<GetContractByIdDto>().Entity<GetContractByIdDto>().HasNoKey();
            modelBuilder.Ignore<GetClassifiedContractByIdDto>().Entity<GetClassifiedContractByIdDto>().HasNoKey();
            modelBuilder.Ignore<GetAllClassifiedContractsDto>().Entity<GetAllClassifiedContractsDto>().HasNoKey();
            modelBuilder.Ignore<GetAddendumContractByIdDto>().Entity<GetAddendumContractByIdDto>().HasNoKey();
            modelBuilder.Ignore<GetAllAuditDto>().Entity<GetAllAuditDto>().HasNoKey();
            modelBuilder.Ignore<GetAllNotificationsDto>().Entity<GetAllNotificationsDto>().HasNoKey();

            modelBuilder.Entity<GetMastersDTO>().HasNoKey();
            modelBuilder.Ignore<GetAllApprovalMatrixContractDTO>().Entity<GetAllApprovalMatrixContractDTO>().HasNoKey();
            modelBuilder.Ignore<GetApprovalMatrixContractByIdDto>().Entity<GetApprovalMatrixContractByIdDto>().HasNoKey();
            modelBuilder.Ignore<GetAllApprovalMatrixMOUByIdDto>().Entity<GetAllApprovalMatrixMOUByIdDto>().HasNoKey();
            modelBuilder.Ignore<GetAllApprovalMatrixMOUDto>().Entity<GetAllApprovalMatrixMOUDto>().HasNoKey();
            modelBuilder.Ignore<ContractsCount>().Entity<ContractsCount>().HasNoKey();

            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.EmployeeCode);
            modelBuilder.Entity<MasterEmployee>().HasAlternateKey(u => u.ValueId);
            //for companymaster unique constraint
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.CompanyName);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.PocEmailId);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.PocEmailId);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.CompanyContactNo);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.CompanyEmailId);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.CompanyWebsiteUrl);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.GSTno);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.BankAccNo);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.MSMERegistrationNo);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.IFSCCode);
            modelBuilder.Entity<MasterCompany>().HasAlternateKey(u => u.PanNo);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterEscalationMatrixContract>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<MasterEscalationMatrixMou>().HasAlternateKey(mamc => mamc.DepartmentId);
            modelBuilder.Entity<AuditTrail>().HasAlternateKey(at => at.ValueId);
            //modelBuilder.Entity<Notification>().HasAlternateKey(n => n.EmployeeCode);
            modelBuilder.Entity<Notification>().HasAlternateKey(n => n.ValueId);

            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixContract>().HasOne(mamc => mamc.Department).WithMany().HasForeignKey(mamc => mamc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);


            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver1).WithMany().HasForeignKey(mamc => mamc.ApproverId1).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver2).WithMany().HasForeignKey(mamc => mamc.ApproverId2).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Approver3).WithMany().HasForeignKey(mamc => mamc.ApproverId3).HasPrincipalKey(me => me.EmployeeCode);
            modelBuilder.Entity<MasterApprovalMatrixMOU>().HasOne(mamc => mamc.Department).WithMany().HasForeignKey(mamc => mamc.DepartmentId).HasPrincipalKey(d => d.DepartmentId);

            modelBuilder.Entity<PostTerminationNotice>().HasOne(ptn => ptn.Contract).WithMany().HasForeignKey(ptn => ptn.ContractId).HasPrincipalKey(c => c.ContractId);
            modelBuilder.Entity<NoticeWithdrawal>().HasOne(nw => nw.Contract).WithMany().HasForeignKey(nw => nw.ContractId).HasPrincipalKey(c => c.ContractId);
            modelBuilder.Entity<NoticeWithdrawal>().HasOne(nw => nw.PostTermination).WithMany().HasForeignKey(nw => nw.TerminationNoticeId).HasPrincipalKey(c => c.ValueId);
            
            modelBuilder.Entity<ClassifiedPostTerminationNotice>().HasOne(ptn => ptn.ClassifiedContract).WithMany().HasForeignKey(ptn => ptn.ClassifiedContractId).HasPrincipalKey(c => c.ClassifiedContractId);
            modelBuilder.Entity<ClassifiedNoticeWithdrawal>().HasOne(nw => nw.ClassifiedContract).WithMany().HasForeignKey(nw => nw.ClassifiedContractId).HasPrincipalKey(c => c.ClassifiedContractId);
            modelBuilder.Entity<ClassifiedNoticeWithdrawal>().HasOne(nw => nw.ClassifiedPostTermination).WithMany().HasForeignKey(nw => nw.TerminationNoticeId).HasPrincipalKey(c => c.ValueId);

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

            //Addendum Contract Relationship with other table. 
            modelBuilder.Entity<AddendumContract>().HasOne(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId).HasPrincipalKey(d => d.DepartmentId);
            modelBuilder.Entity<AddendumContract>().HasOne(c => c.ContractWithCompany).WithMany().HasForeignKey(c => c.ContractWithCompanyId).HasPrincipalKey(cc => cc.ValueId);
            modelBuilder.Entity<AddendumContract>().HasOne(c => c.ContractType).WithMany().HasForeignKey(c => c.ContractTypeId).HasPrincipalKey(ct => ct.ValueId);
            modelBuilder.Entity<AddendumContract>().HasOne(c => c.ApostilleType).WithMany().HasForeignKey(c => c.ApostilleTypeId).HasPrincipalKey(at => at.ValueId);
            modelBuilder.Entity<AddendumContract>().HasOne(c => c.EmpCustodian).WithMany().HasForeignKey(c => c.EmpCustodianId).HasPrincipalKey(ec => ec.ValueId);

            modelBuilder.Entity<ClassifiedContract>().HasOne(c => c.Department).WithMany().HasForeignKey(c => c.DepartmentId).HasPrincipalKey(d => d.DepartmentId);
            modelBuilder.Entity<ClassifiedContract>().HasOne(c => c.ContractWithCompany).WithMany().HasForeignKey(c => c.ContractWithCompanyId).HasPrincipalKey(cc => cc.ValueId);
            modelBuilder.Entity<ClassifiedContract>().HasOne(c => c.ContractType).WithMany().HasForeignKey(c => c.ContractTypeId).HasPrincipalKey(ct => ct.ValueId);
            modelBuilder.Entity<ClassifiedContract>().HasOne(c => c.ApostilleType).WithMany().HasForeignKey(c => c.ApostilleTypeId).HasPrincipalKey(at => at.ValueId);
            modelBuilder.Entity<ClassifiedContract>().HasOne(c => c.EmpCustodian).WithMany().HasForeignKey(c => c.EmpCustodianId).HasPrincipalKey(ec => ec.ValueId);

            modelBuilder.Entity<AuditTrail>().HasOne(at => at.Employee).WithMany().HasForeignKey(at => at.LoggedBy).HasPrincipalKey(me => me.EmployeeCode);

            // Configurations and Data seeding
            modelBuilder.ApplyConfiguration(new ApostilleConfiguration());
            modelBuilder.ApplyConfiguration<ListOfCountries>(new CompanyCascadeConfiguration());
            modelBuilder.ApplyConfiguration<ListOfStates>(new CompanyCascadeConfiguration());
            modelBuilder.ApplyConfiguration<ListofCity>(new CompanyCascadeConfiguration());
            modelBuilder.ApplyConfiguration(new ContractTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new MasterEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EscalationMatrixContractConfiguration());
            modelBuilder.ApplyConfiguration(new EscalationMatrixMOUConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalMatrixContractConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalMatrixMOUConfiguration());

        }
    }
}
