using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class AddendumContractRepository : IAddendumContractRepository
    {
        private readonly CMSDbContext _dbContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;

        public AddendumContractRepository(CMSDbContext dbContext, INotificationRepository notificationRepository, IEmailService emailService)
        {
            _dbContext = dbContext;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
        }

        public async Task<AddendumContract> AddAddendumContractAsync(int id, AddendumContract addendumContract)
        {
            var contractIdCheck = await _dbContext.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (contractIdCheck == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }

            //contractIdCheck.ContractName = cp.ContractName;
            contractIdCheck.DepartmentId = addendumContract.DepartmentId;
            contractIdCheck.ContractWithCompanyId = addendumContract.ContractWithCompanyId;
            contractIdCheck.ContractTypeId = addendumContract.ContractTypeId;
            contractIdCheck.ApostilleTypeId = addendumContract.ApostilleTypeId;
            contractIdCheck.ActualDocRefNo = addendumContract.ActualDocRefNo;
            contractIdCheck.RetainerContract = addendumContract.RetainerContract;
            contractIdCheck.TermsAndConditions = addendumContract.TermsAndConditions;
            contractIdCheck.ValidFrom = addendumContract.ValidFrom;
            contractIdCheck.ValidTill = addendumContract.ValidTill;
            contractIdCheck.EmpCustodianId = addendumContract.EmpCustodianId;
            contractIdCheck.IsDeleted = addendumContract.IsDeleted;

            ////Employee Details 
            //contractIdCheck.EmployeeName = addendumContract.EmployeeName;
            //contractIdCheck.EmployeeCode = addendumContract.EmployeeCode;
            //contractIdCheck.EmployeeMobile = addendumContract.EmployeeMobile;
            //contractIdCheck.Email = addendumContract.Email;
            //contractIdCheck.Role = addendumContract.Role;
            //contractIdCheck.EmpDepartmentId = addendumContract.EmpDepartmentId;
            //contractIdCheck.Unit = addendumContract.Unit;


            ////Company Details 
            //contractIdCheck.CompanyName = addendumContract.CompanyName;
            //contractIdCheck.CompanyContactNo = addendumContract.CompanyContactNo;
            //contractIdCheck.CompanyEmailId = addendumContract.CompanyEmailId;
            //contractIdCheck.CompanyAddressLine1 = addendumContract.CompanyAddressLine1;
            //contractIdCheck.CompanyAddressLine2 = addendumContract.CompanyAddressLine2;
            //contractIdCheck.CompanyAddressLine3 = addendumContract.CompanyAddressLine3;
            //contractIdCheck.Zipcode = addendumContract.Zipcode;
            //contractIdCheck.CountryId = addendumContract.CountryId;
            //contractIdCheck.StateId = addendumContract.StateId;
            //contractIdCheck.CityId = addendumContract.CityId;
            //contractIdCheck.PocName = addendumContract.PocName;
            //contractIdCheck.PocContactNumber = addendumContract.PocContactNumber;
            //contractIdCheck.PocEmailId = addendumContract.PocEmailId;

            _dbContext.ContractsEntity.Update(contractIdCheck);
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _dbContext.GetContractByIdDtos.FromSqlRaw(sql, contractIdCheck.ContractId).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                // To Approver L1
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          $"New Addendum to Contract called '{foundContract.ContractName}' Added!",
                                          $"New Addendum has been added under the Contract created by {foundContract.EmpCustodianCode}. You can access and change the approvals for this contract.");
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, contractIdCheck.ContractId, contractIdCheck.ContractName
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Addendum under the Contract '{foundContract.ContractName}'!",
                                          "New Addendum has been successfully added for your contract.");
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, contractIdCheck.ContractId, contractIdCheck.ContractName
                );
                return addendumContract;
            }
            throw new Exception("Failed to add Addendum Contract");
        }

        private async Task AddNewNotifications(string name, string subject, string message)
        {
            Notification createNewNotif = new Notification
            {
                EmployeeCode = name,
                NotficationSubject = subject,
                NotficationMessage = message
            };
            await _notificationRepository.NewNotification(createNewNotif);
            var existing = _dbContext.ChangeTracker.Entries<Notification>().FirstOrDefault(e => e.Entity.EmployeeCode == name);
            if (existing != null)
                existing.State = EntityState.Detached;
        }
        private string GenerateEmailBody(string name, int contractID, string contractName)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>Hello {name}, new addendum has been added under the contract {contractName}.</h1>";
            emailBody += $"<h2>Contract ID: {contractID}<br>Contract Name: {contractName}</h2>";
            emailBody += "<h3>Please check your CMS portal.</h3>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task SendMail(string email, string name, int contractID, string contractName)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = "New Addendum Added",
                EmailBody = GenerateEmailBody(name, contractID, contractName)
            };
            await _emailService.SendEmail(mailRequest);
        }
    }
}
