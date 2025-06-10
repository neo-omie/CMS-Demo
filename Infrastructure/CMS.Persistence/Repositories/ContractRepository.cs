using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using CMS.Application.Contracts.Persistence;
using CMS.Application.DTOs;
using CMS.Application.Exceptions;
using CMS.Application.Features.Contracts.Queries.GetAllContracts;
using CMS.Application.Features.Contracts.Queries.GetContractById;
using CMS.Domain.Constants;
using CMS.Domain.Entities;
using CMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CMS.Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        readonly CMSDbContext _context;
        readonly IEmailService _emailService;
        readonly INotificationRepository _notificationRepository;
        public ContractRepository(CMSDbContext context, IEmailService emailService, INotificationRepository notificationRepository)
        {
            _emailService = emailService;
            _context = context;
            _notificationRepository = notificationRepository;
        }
        public async Task<ContractsCount> GetContractsCountAsync()
        {
            string sql = "EXEC SP_ContractsCounts";
            var counter = _context.ContractsCounter.FromSqlRaw(sql).AsNoTracking().ToList();
            var allCounters = counter.FirstOrDefault();
            if (allCounters == null)
                throw new NotFoundException("Contracts count not found for some reason");
            return allCounters;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetAllContractsAsync(FiltersContractDto filters)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetAllContractsEntity @PageNumber = {0}, @PageSize = {1}, @SearchTerm = {2}, " +
                         "@FromDate = {3}, @ToDate = {4}, @ContractType = {5}, @RenewalDueIn = {6}, " +
                         "@ContractStatus = {7}, @Department = {8}, @Location = {9}, @HasAddendum = {10}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, filters.PageNumber, filters.PageSize,
                filters.SearchTerm, filters.FromDate, filters.ToDate, filters.ContractType, filters.RenewalDueIn,
                filters.ContractStatus, filters.Department, filters.Location, filters.HasAddendum).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetActiveContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetActiveContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetTerminatedContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetTerminatedContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetPendingApprovalContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetPendingApprovalContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }
        public async Task<IEnumerable<GetAllContractsDto>> GetExpiredContractsAsync(int pageNumber, int pageSize)
        {
            int totalRecords = await _context.ContractsEntity.Where(x => x.IsDeleted == false).CountAsync();
            string sql = "EXEC SP_GetExpiredContractsEntity @PageNumber = {0}, @PageSize = {1}";
            var allContracts = await _context.GetContractsDtos.FromSqlRaw(sql, pageNumber, pageSize).ToListAsync();
            return allContracts;
        }

        public async Task<GetContractByIdDto> GetContractByIdAsync(int id)
        {
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if(foundContract == null)
            {
                throw new NotFoundException($"Contract with ID {id} not found");
            }
            return foundContract;

        }
        public async Task<GetContractByIdDto> GetContractByNameAsync(string name)
        {
            string sql = "EXEC SP_GetContractEntityByName @Name = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, name).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with name {name} not found");
            }
            return foundContract;

        }
        public async Task<Contract> AddContractAsync(Contract cp,string empName)
        {
            var addedContract = await _context.ContractsEntity.AddAsync(cp);
            if(await _context.SaveChangesAsync() <= 0)
                throw new Exception("For some reasons, contract has not been added.");
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, cp.ContractId).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            string empCode;
            if(foundContract != null)
            {
                empCode = foundContract.Approver1EmployeeCode;
                // To Approver L1
                await AddNewNotifications(empCode,
                                          $"New Contract called '{foundContract.ContractName}' Added!",
                                          "New Contract has been added under your department. You can access and change the approvals for this contract.");
                string subject = $"Contract Added";
                string emailBody = string.Empty;
                emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                emailBody += $"<h1>Hello {foundContract.Approver1EmployeeCode}, new contract has been started under your department.</h1>";
                emailBody += $"<h2>Contract ID: {cp.ContractId}<br>Contract Name: {cp.ContractName}</h2>";
                emailBody += "<h3>Please check your CMS portal.</h3>";
                emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                emailBody += "</div>";
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, cp.ContractId, cp.ContractName, subject, emailBody
                );
                // To Employee Custodian
                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          $"You have added new Contract called '{foundContract.ContractName}'!",
                                          "New Contract has been added under your department.");
                emailBody = string.Empty;
                emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                emailBody += $"<h1>Hello {foundContract.EmpCustodianCode}, new contract has been started under your department.</h1>";
                emailBody += $"<h2>Contract ID: {cp.ContractId}<br>Contract Name: {cp.ContractName}</h2>";
                emailBody += "<h3>Please check your CMS portal.</h3>";
                emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                emailBody += "</div>";
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, cp.ContractId, cp.ContractName,subject, emailBody
                );

                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, " New Contract created by " + empCode, empName, LogStatus.Created);

            }
            return cp;
        }
        public async Task<Contract> ApproveRejectContract(int id, string empCode, ContractStatus status)
        {
            var contract = await _context.ContractsEntity.Where(c => c.ContractId == id).FirstOrDefaultAsync();
            var emp = await _context.MasterEmployees.Where(e => e.Email == empCode).FirstOrDefaultAsync();
            if (contract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql,id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            string approveOrReject = (status == ContractStatus.Active) ? "Approved" : "Rejected";
            string notificationSubject = (status == ContractStatus.Active) ? "Contract has been approved under your department. You can access and change the approvals for this contract." : "Contract has been rejected under your department.";

            if (foundContract.Approver1Email == empCode)
            {

                if(contract.Approver1Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 1";
                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                string emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ContractName,id,approveOrReject, foundContract.Approver1EmployeeCode,1);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver2EmployeeCode, id, foundContract.ContractName,subject,emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0]+".",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName,subject,emailBody
                );
            }
            else if(foundContract.Approver2Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.Active || contract.Approver2Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 2";
                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode,
                                          notificationSubject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                string emailBody = GenerateEmailBody(foundContract.Approver3EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver3Email, foundContract.Approver3EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0]+" by approver 2.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0]+".",
                                            $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName, subject, emailBody
                );
            }
            else if( foundContract.Approver3Email == empCode)
            {
                if (contract.Approver2Status != ContractStatus.Active || contract.Approver1Status != ContractStatus.Active || contract.Approver3Status != ContractStatus.PendingApproval)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 3";
                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0]+" by approver 3.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                string emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 3.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                            $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName, subject, emailBody
                );
            }
            else
            {
                throw new Exception("Unauthorized Action");
            }
            if (status == ContractStatus.Rejected)
            {
                contract.Approver1Status = status;
                contract.Approver2Status = status;
                contract.Approver3Status = status;

                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ContractId, TableList.Contract, "Contract Rejected by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Rejected);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reasons , contract status has not been changed to {status}");
                }
                return contract;
            }

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, $"Contract Approved by  '{emp.EmployeeCode}'", emp.EmployeeCode, LogStatus.Approved);


            return contract;
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
            var existing = _context.ChangeTracker.Entries<Notification>().FirstOrDefault(e => e.Entity.EmployeeCode == name);

            if (existing != null)
            {
                existing.State = EntityState.Detached;
            }

        }
        private string GenerateEmailBody(string empCode, string contractName , int contractID, string approveOrReject, string approverCode, int approverLevel)
        {
            string emailBody = string.Empty;
            emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
            emailBody += $"<h1>Hello {empCode},</h1>";
            emailBody += $"<h2>Contract called '{contractName}'({contractID}), {approveOrReject} by '{approverCode}'(Approver {approverLevel})!.</h2>";
            emailBody += "<h3>Please check your CMS portal.</h3>";
            emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
            emailBody += "</div>";
            return emailBody;
        }
        public async Task   SendMail(string email, string name, int contractID, string contractName, string subject, string emailBody)
        {
            var mailRequest = new MailRequest
            {
                Email = email,
                Subject = subject,
                EmailBody = emailBody
            };
            await _emailService.SendEmail(mailRequest);
        }
        public async Task<bool> DeleteContractAsync(int id, string empCode)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            foundContract.IsDeleted = true;
            _context.ContractsEntity.Update(foundContract);

            if (await _context.SaveChangesAsync() > 0)
            {

                string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, $"Contract  '{foundContract.ContractName}' Deleted by '{empCode}'", empCode, LogStatus.Deleted);


                return true;
            }
            return false;
        }


        public async Task<bool> UpdateContractAsync(int id, Contract cp)
        {
            var foundContract = await _context.ContractsEntity.FirstOrDefaultAsync(ce => ce.ContractId == id);
            if (foundContract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            foundContract.ContractName = cp.ContractName;
            foundContract.DepartmentId = cp.DepartmentId;
            foundContract.ContractWithCompanyId = cp.ContractWithCompanyId;
            foundContract.ContractTypeId = cp.ContractTypeId;
            foundContract.ApostilleTypeId = cp.ApostilleTypeId;
            foundContract.ActualDocRefNo = cp.ActualDocRefNo;
            foundContract.RetainerContract = cp.RetainerContract;
            foundContract.TermsAndConditions = cp.TermsAndConditions;
            foundContract.ValidFrom = cp.ValidFrom;
            foundContract.ValidTill = cp.ValidTill;
            foundContract.RenewalFrom = cp.RenewalFrom;
            foundContract.RenewalTill = cp.RenewalTill;
            foundContract.AddendumDate = cp.AddendumDate;
            foundContract.EmpCustodianId = cp.EmpCustodianId;
            foundContract.Location = cp.Location;
            foundContract.Approver1Status = cp.Approver1Status;
            foundContract.Approver2Status = cp.Approver2Status;
            foundContract.Approver3Status = cp.Approver3Status;
            foundContract.IsDeleted = cp.IsDeleted;
            _context.ContractsEntity.Update(foundContract);
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<Contract> RenewalRequestContractAsync(int id, string employeeCode)
        {
            Contract contract = await _context.ContractsEntity.Where(c => c.ContractId == id).FirstOrDefaultAsync();
            if(contract == null)
            {
                throw new NotFoundException($"Contract with id {id} not found. Please enter correct id");
            }
            var employee = await _context.MasterEmployees.Where(e => e.ValueId == contract.EmpCustodianId).FirstOrDefaultAsync();
            if (employee == null) 
            {
                throw new NotFoundException($"Employee not found.");
            }
            if(contract.RenewalFrom <= DateTime.Now && contract.RenewalTill >= DateTime.Now && (employee.EmployeeCode == employeeCode || employeeCode == "NEO1") &&
                (contract.Approver1Status == ContractStatus.Active || contract.Approver1Status == ContractStatus.Expired) &&
               (contract.Approver1Status == ContractStatus.Active || contract.Approver2Status == ContractStatus.Expired) &&
              (contract.Approver3Status == ContractStatus.Active || contract.Approver3Status == ContractStatus.Expired))
            {
                contract.Approver1Status = ContractStatus.PendingRenewal;
                contract.Approver2Status = ContractStatus.PendingRenewal;
                _context.ContractsEntity.Update(contract);
                if (await _context.SaveChangesAsync() <= 0)
                    throw new Exception("For some reasons, renewal request not send.");
                string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
                var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, contract.ContractId).AsNoTracking().ToListAsync();
                var foundContract = findingContract.FirstOrDefault();
                string empCode;
                if (foundContract != null)
                {
                    empCode = foundContract.Approver1EmployeeCode;
                    // To Approver L1
                    await AddNewNotifications(empCode,
                                              $"Renewal request for Contract called '{foundContract.ContractName}'",
                                              $"Contract ({foundContract.ContractName}) has been requested for renewal under your department. You can access and change the approvals for this contract.");
                    string subject = $"Renewal request for Contract called '{foundContract.ContractName}'";
                    string emailBody = string.Empty;
                    emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                    emailBody += $"<h1>Hello {foundContract.Approver1EmployeeCode},contract has been requested for renewal under your department.</h1>";
                    emailBody += $"<h2>Contract ID: {contract.ContractId}<br>Contract Name: {contract.ContractName}</h2>";
                    emailBody += "<h3>Please check your CMS portal.</h3>";
                    emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                    emailBody += "</div>";
                    await SendMail(
                        foundContract.Approver1Email, foundContract.Approver1EmployeeCode, contract.ContractId, contract.ContractName, subject, emailBody
                    );
                    // To Employee Custodian
                    await AddNewNotifications(foundContract.EmpCustodianCode,
                                              $"You have requested to renew Contract called '{foundContract.ContractName}'!",
                                              $"Contract ({foundContract.ContractName}) has been requested for renewal by you.");
                    emailBody = string.Empty;
                    emailBody = "<div style='width: 100%; background-color: #5f5fee; color: white;'>";
                    emailBody += $"<h1>Hello {foundContract.EmpCustodianCode}, contract has been requested for renewal by you.</h1>";
                    emailBody += $"<h2>Contract ID: {contract.ContractId}<br>Contract Name: {contract.ContractName}</h2>";
                    emailBody += "<h3>Please check your CMS portal.</h3>";
                    emailBody += "<p>Thank you,<br>Regards, Trailblazers.</p>";
                    emailBody += "</div>";
                    await SendMail(
                        foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, contract.ContractId, contract.ContractName, subject, emailBody
                    );

                    string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                    await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, $"Contract({contract.ContractName}) renew request send by" + empCode, empCode, LogStatus.Updated);

                    return contract;
                }

            }
            throw new Exception("Unauthorize action");
        }
        public async Task<Contract> ApproveRejectRenewalRequestContract(int id, string empCode, ContractStatus status)
        {
            var contract = await _context.ContractsEntity.Where(c => c.ContractId == id).FirstOrDefaultAsync();
            var emp = await _context.MasterEmployees.Where(e => e.Email == empCode).FirstOrDefaultAsync();
            if (contract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            string sql = "EXEC SP_GetContractEntityByID @ID = {0}";
            var findingContract = await _context.GetContractByIdDtos.FromSqlRaw(sql, id).AsNoTracking().ToListAsync();
            var foundContract = findingContract.FirstOrDefault();
            if (foundContract == null)
            {
                throw new NotFoundException("Contract not found");
            }
            string approveOrReject = (status == ContractStatus.Active) ? "Renewal approved" : "Renewal rejected";
            string notificationSubject = (status == ContractStatus.Active) ? "Contract renewal has been approved under your department. You can access and change the approvals for this contract." : "Contract renewal has been rejected under your department.";

            if (foundContract.Approver1Email == empCode)
            {

                if (contract.Approver1Status != ContractStatus.PendingRenewal)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 1";
                contract.Approver1Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not updated");
                }

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                string emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver2EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver1EmployeeCode}'(Approver 1)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver1EmployeeCode, 1);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName, subject, emailBody
                );
            }
            else if (foundContract.Approver2Email == empCode)
            {
                if (contract.Approver1Status != ContractStatus.Active || contract.Approver2Status != ContractStatus.PendingRenewal)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 2";
                contract.Approver2Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }

                await AddNewNotifications(foundContract.Approver3EmployeeCode,
                                          notificationSubject,
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                string emailBody = GenerateEmailBody(foundContract.Approver3EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver3Email, foundContract.Approver3EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 2.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                            $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver2EmployeeCode}'(Approver 2)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver2EmployeeCode, 2);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName, subject, emailBody
                );
            }
            else if (foundContract.Approver3Email == empCode)
            {
                if (contract.Approver2Status != ContractStatus.Active || contract.Approver1Status != ContractStatus.Active || contract.Approver3Status != ContractStatus.Active || contract.Approver3Status != ContractStatus.Expired)
                {
                    throw new Exception("Invalid approval action");
                }
                string subject = $"Contract:{foundContract.ContractName}({id}),{approveOrReject} by Approver 3";
                contract.Approver3Status = status;

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception("For some reasons, contract is not approved");
                }
                await AddNewNotifications(foundContract.Approver1EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 3.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                string emailBody = GenerateEmailBody(foundContract.Approver1EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver1Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.Approver2EmployeeCode,
                                          notificationSubject.Split('.')[0] + " by approver 3.",
                                          $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.Approver2EmployeeCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.Approver2Email, foundContract.Approver1EmployeeCode, id, foundContract.ContractName, subject, emailBody
                );

                await AddNewNotifications(foundContract.EmpCustodianCode,
                                          notificationSubject.Split('.')[0] + ".",
                                            $"Contract called '{foundContract.ContractName}' {approveOrReject} by '{foundContract.Approver3EmployeeCode}'(Approver 3)!");
                emailBody = GenerateEmailBody(foundContract.EmpCustodianCode, foundContract.ContractName, id, approveOrReject, foundContract.Approver3EmployeeCode, 3);
                await SendMail(
                    foundContract.EmpCustodianEmail, foundContract.EmpCustodianCode, id, foundContract.ContractName, subject, emailBody
                );
            }
            else
            {
                throw new Exception("Unauthorized Action");
            }
            if (status == ContractStatus.Rejected)
            {
                contract.Approver1Status = status;
                contract.Approver2Status = status;
                contract.Approver3Status = status;

                string rejectedQuery = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
                await _context.Database.ExecuteSqlRawAsync(rejectedQuery, foundContract.ContractId, TableList.Contract, "Contract Rejected by " + emp.EmployeeCode, emp.EmployeeCode, LogStatus.Rejected);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    throw new Exception($"For some reasons , contract status has not been changed to {status}");
                }
                return contract;
            }

            string query = "EXEC SP_InsertAudit @TableId = {0}, @ForTable = {1}, @ActionDescription = {2}, @LoggedBy = {3}, @Status = {4}";
            await _context.Database.ExecuteSqlRawAsync(query, foundContract.ContractId, TableList.Contract, $"Contract Approved by  '{emp.EmployeeCode}'", emp.EmployeeCode, LogStatus.Approved);


            return contract;
        }
    }
}
