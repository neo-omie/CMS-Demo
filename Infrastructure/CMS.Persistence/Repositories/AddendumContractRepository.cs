using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Application.Features.AddendumContract.AddendumContractDto;
using CMS.Application.Features.AddendumContracts.AddendumContractDto;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Constants;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CMS.Persistence.Repositories
{
    public class AddendumContractRepository :IAddendumContractRepository
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

        public async Task<(IEnumerable<AddendumContract> Data, int TotalCount)> GetAllAddendumContractsAsync(int pageNumber, int pageSize, DateTime? searchTerm)
        {
            var query = _dbContext.AddendumContracts.AsQueryable();
            var search = searchTerm.ToString();

            if (!string.IsNullOrEmpty(search))
            {
                var IsInt = DateTime.TryParse(search, out DateTime date);
                if (IsInt)
                    query = query.Where(e => e.AddendumDate == date);
            }

            query = query.Where(x => x.IsDeleted == false);
            int totalCount = await query.CountAsync();

            var data = await query
            .Where(x => x.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(x=>x.AddendumDate)
            .ToListAsync();

            return (data, totalCount);
        }

        public async Task<(IEnumerable<AddendumContract> Data, int TotalCount)> GetAllAddendumByContractIdAsync(int pageNumber, int pageSize, int id) {
            var gotContract = await _dbContext.AddendumContracts.Where(mc => mc.ContractId == id && mc.IsDeleted == false).ToListAsync();
            if(gotContract == null)
            {
                throw new NotFoundException($"Contract with this id is not available.");
            }

            var data = await _dbContext.AddendumContracts
            .Where(mc => mc.ContractId == id && mc.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            int totalCount = await _dbContext.AddendumContracts.Where(mc => mc.ContractId == id && mc.IsDeleted == false).CountAsync();

            return (data, totalCount);
        }

        public async Task<GetAddendumContractByIdDto> GetAddendumByAddendumContractIdAsync(int id)
        {
            var sql = "EXEC SP_GetAddendumContractByID @ID = {0}";
            var gotAddendum = _dbContext.GetAddendumContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToList();
            if (gotAddendum == null)
                throw new NotFoundException($"Addendum with this id is not available.");
            var showAddendum = gotAddendum.FirstOrDefault();
            return showAddendum;
        }

        public async Task<bool> DeleteAddendumContractAsync(int id, string empCode)
        {
            var addendum = await _dbContext.AddendumContracts.FirstOrDefaultAsync(me => me.AddendumContractId == id);
            if(addendum == null)
            {
                throw new NotFoundException($"Addendum with this id is not available.");
            }
            addendum.IsDeleted = true;
            _dbContext.AddendumContracts.Update(addendum);

            if(await _dbContext.SaveChangesAsync() > 0)
            {
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _dbContext.Database.ExecuteSqlRawAsync(query, id, TableList.AddendumContract, $" Addendum  '{addendum.ContractName}' has been Deleted by '{empCode}'", empCode, LogStatus.Deleted);

                return true;
            }
            else { return false; }
        }

        public async Task<AddAddendumContractDto> AddAddendumContractAsync(int id, AddAddendumContractDto addendumContract, string empCode)
        {
            var contractIdCheck = await _dbContext.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id && ce.Approver3Status == ContractStatus.Active);


            if (contractIdCheck == null)    
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }

            var newAddendumContract = new AddendumContract();

            newAddendumContract.ContractId = id;
            newAddendumContract.ContractName = addendumContract.ContractName;
            newAddendumContract.DepartmentId = addendumContract.DepartmentId;
            newAddendumContract.ContractWithCompanyId = addendumContract.ContractWithCompanyId;
            newAddendumContract.ContractTypeId = addendumContract.ContractTypeId;
            newAddendumContract.ApostilleTypeId = addendumContract.ApostilleTypeId;
            newAddendumContract.ActualDocRefNo = addendumContract.ActualDocRefNo;
            newAddendumContract.RetainerContract = addendumContract.RetainerContract;
            newAddendumContract.TermsAndConditions = addendumContract.TermsAndConditions;
            newAddendumContract.ValidFrom = addendumContract.ValidFrom;
            newAddendumContract.ValidTill = addendumContract.ValidTill;
            newAddendumContract.EmpCustodianId = addendumContract.EmpCustodianId;
            newAddendumContract.Location = addendumContract.Location;
            newAddendumContract.AddendumDate = addendumContract.AddendumDate;
            newAddendumContract.Approver1Status = addendumContract.Approver1Status;
            newAddendumContract.Approver2Status = addendumContract.Approver2Status;
            newAddendumContract.Approver3Status = addendumContract.Approver3Status;


            //contractIdCheck.AddendumContractId = addendumContract.AddendumContractId;
            //newAddendumContract.ContractId = addendumContract.ContractId;
            //contractIdCheck.IsDeleted = addendumContract.IsDeleted;

            await _dbContext.AddendumContracts.AddAsync(newAddendumContract);

            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _dbContext.GetContractByIdDtos.FromSqlRaw(sql, contractIdCheck.ContractId).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                // To Approver L1
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          $"New Addendum to Contract called '{foundContract.ContractName}' Added!",
                                          $"New Addendum has been added under the Contract created by {foundContract.EmpCustodianCode}. You can Approve or Reject this contract by visiting to the Portal.");
                await SendMailAdd(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, contractIdCheck.ContractId, contractIdCheck.ContractName
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Addendum under the Contract '{foundContract.ContractName}'!",
                                          "New Addendum has been successfully added for your contract.");
                await SendMailAdd(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, contractIdCheck.ContractId, contractIdCheck.ContractName
                );
                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _dbContext.Database.ExecuteSqlRawAsync(query, newAddendumContract.AddendumContractId, TableList.AddendumContract, $" Addendum  '{foundContract.ContractName}'  has been Added by '{empCode}'", empCode, LogStatus.Created);

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
            {
                existing.State = EntityState.Detached;
            }
        }

        private string GenerateEmailBodyAdd(string name, int contractID, string contractName)
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

        public async Task SendMailAdd(string email, string name, int contractID, string contractName)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = "New Addendum Added",
                EmailBody = GenerateEmailBodyAdd(name, contractID, contractName)
            };
            await _emailService.SendEmail(mailRequest);
        }


        private string GenerateEmailBody(string empCode, string contractName, int contractID, string approveOrReject, string approverCode, int approverLevel)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>Hello {empCode}, new addendum has been added under the contract {contractName}.</h1>";
            emailBody += $"<h2>Contract called '{contractName}'({contractID}), {approveOrReject} by '{approverCode}'(Approver {approverLevel})!.</h2>";
            emailBody += "<h3>Please check your CMS portal.</h3>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task SendMail(string email, string name, int contractID, string contractName, string subject, string emailBody)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = subject,
                EmailBody = emailBody
            };
            await _emailService.SendEmail(mailRequest);
        }

        public async Task<AddendumContract> ApproveRejectAddendum(int contractId, ContractStatus addendumStatus, int addendumId, string empCode)
        {
            var contract = await _dbContext.ContractsEntity.Where(c => c.ContractId == contractId).FirstOrDefaultAsync();
            var employee = await _dbContext.MasterEmployees.Where(c => c.Email == empCode).FirstOrDefaultAsync();
            if (contract == null)
            {
                throw new NotFoundException("Contract not found");
            }

            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _dbContext.GetContractByIdDtos.FromSqlRaw(sql, contractId).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault(); 

            if (foundContract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            var addedum = await _dbContext.AddendumContracts.Where(a => a.AddendumContractId == addendumId).FirstOrDefaultAsync();
            if (addedum == null)
            {
                throw new NotFoundException("Addendum not found");
            }

            string subject = $"Approval for contract {foundContract.ContractId} addendum";

            string approveOrReject = (addendumStatus == ContractStatus.Active) ? "Approved" : "Rejected";

            string notificationSubject = (addendumStatus == ContractStatus.Active) ? $"Addendum for contract {addedum.ContractName} has been approved under your department." : $"Addendum for this contract {addedum.ContractName} has been rejected under your department.";


            if (foundContract.Approver1Email == empCode && addedum.Approver1Status == ContractStatus.PendingApproval && addedum.Approver2Status == ContractStatus.PendingApproval && addedum.Approver3Status == ContractStatus.PendingApproval)
            {
               
                addedum.Approver1Status = addendumStatus;
                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }
                await AddNewNotifications(foundContract.Approver2EmployeeCode,subject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                string emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver3EmployeeCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode, subject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");

                emailBody = GenerateEmailBody(foundContract.EmpCustodianEmail, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver1EmployeeCode, 1);

                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );

            }
            else if(foundContract.Approver2Email == empCode && addedum.Approver1Status == ContractStatus.Active && addedum.Approver2Status == ContractStatus.PendingApproval && addedum.Approver3Status == ContractStatus.PendingApproval)
            {
                addedum.Approver2Status = addendumStatus;
                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }
                await AddNewNotifications(foundContract.Approver3EmployeeCode, subject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");

                string emailBody = GenerateEmailBody(foundContract.Approver3EmployeeCode, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver2EmployeeCode, 2);

                await SendMail(
                    foundContract.Approver3Email, foundContract.Approver3EmployeeCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode, subject,
                                         $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");

                emailBody = GenerateEmailBody(foundContract.EmpCustodianEmail, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver2EmployeeCode, 2);

                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );
               
            }
            else if(foundContract.Approver3Email == empCode && addedum.Approver1Status == ContractStatus.Active && addedum.Approver2Status == ContractStatus.Active && addedum.Approver3Status == ContractStatus.PendingApproval)
            {
                addedum.Approver3Status = addendumStatus;
                contract.ContractName = addedum.ContractName;
                contract.DepartmentId = addedum.DepartmentId;
                contract.ContractWithCompanyId = addedum.ContractWithCompanyId;
                contract.ContractTypeId = addedum.ContractTypeId;
                contract.ApostilleTypeId = addedum.ApostilleTypeId;
                contract.ActualDocRefNo = addedum.ActualDocRefNo;
                contract.RetainerContract = addedum.RetainerContract;
                contract.TermsAndConditions = addedum.TermsAndConditions;
                contract.ValidFrom = addedum.ValidFrom;
                contract.ValidTill = addedum.ValidTill;
                contract.AddendumDate = addedum.AddendumDate;
                contract.EmpCustodianId = addedum.EmpCustodianId;
                contract.Location = addedum.Location;
                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }
                await AddNewNotifications(foundContract.Approver3EmployeeCode, subject,
                                         $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");

                string emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver3EmployeeCode, 3);

                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver3EmployeeCode, subject,
                                         $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");

                emailBody = GenerateEmailBody(foundContract.Approver3EmployeeCode, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver3EmployeeCode, 3);

                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver2EmployeeCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver3EmployeeCode, subject,
                                         $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");

                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, foundContract.ContractId, approveOrReject, foundContract.Approver3EmployeeCode, 3);

                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, foundContract.ContractId, foundContract.ContractName, subject, emailBody
                );
               
            }
            else
            {
                throw new Exception($"Unauthorized Approval Action");
            }
            if (addendumStatus == ContractStatus.Rejected)
            {
                contract.Approver1Status = addendumStatus;
                contract.Approver2Status = addendumStatus;
                contract.Approver3Status = addendumStatus;

                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _dbContext.Database.ExecuteSqlRawAsync(rejectedQuery, addendumId, TableList.AddendumContract, $"Addendum  '{foundContract.ContractName}' has been Rejected by '{empCode}'", empCode, LogStatus.Rejected);
                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reasons , contract status has not been changed to {addendumStatus}");
                }

            }
            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _dbContext.Database.ExecuteSqlRawAsync(query, addendumId, TableList.AddendumContract, $"Addendum  '{foundContract.ContractName}' has been Approved by '{employee.EmployeeCode}'", employee.EmployeeCode, LogStatus.Approved);

            return addedum;
        }
        //private string GenerateEmailBodyAddendum(string empCode, string contractName, int contractID, string approveOrReject, string approverCode, int approverLevel)
        //{
        //    string emailBody = string.Empty;
        //    emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
        //    emailBody += $"<h1>Hello {empCode},</h1>";
        //    emailBody += $"<h2>Contract called '{contractName}'({contractID}), {approveOrReject} by '{approverCode}'(Approver {approverLevel})!.</h2>";
        //    emailBody += "<h3>Please check your CMS portal.</h3>";
        //    emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
        //    emailBody += "</div>";
        //    return emailBody;
        //}
        //public async Task SendMailAddendum(string email, string name, int contractID, string contractName, string subject, string emailBody)
        //{
        //    var mailRequest = new MailRequest
        //    {
        //        Email = email,
        //        Subject = subject,
        //        EmailBody = emailBody
        //    };
        //    await _emailService.SendEmail(mailRequest);
        //}

    }
}
